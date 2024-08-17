using Azure;
using AzureAccess.Interfaces;
using Data;
using DataAccess.Abstractions;
using DataAccess.DAL;
using DataAccess.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Services;
using Google.Protobuf;
using Grpc.Core;
using MongoDB.Bson;

namespace Retrieval.Services;

public class DocumentService : DocumentProto.DocumentProtoBase
{
    private readonly ILogger<DocumentService> _logger;
    private readonly IEntityService<Document> _entityService;
    private readonly IFileService _fileService;
    private const int ChunkSize = 1024 * 32;

    public DocumentService( ILogger<DocumentService> logger, 
                            IEntityService<Document> entityService, 
                            IFileService fileService
                          )
    {
        _logger = logger;
        _entityService = entityService;
        _fileService = fileService;
    }

    public override async Task Retrieve(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {
        var doc = await _entityService.Get(ObjectId.Parse(request.Id));

        try
        {
            var file = await _fileService.GetFile(doc.File.FileName);
            file.Position = 0;
            
            var buffer = new byte[ChunkSize];
            
            while (true)
            {
                var numBytesRead = await file.ReadAsync(buffer);
                if (numBytesRead == 0)
                {
                    break;
                }

                _logger.LogInformation("Sending data chunk of {numBytesRead} bytes", numBytesRead);
                await responseStream.WriteAsync(new Response()
                {
                    Data = UnsafeByteOperations.UnsafeWrap(buffer.AsMemory(0, numBytesRead))
                }) ;
            }
        }
        catch (Exception e)
        {
            throw new DataAccessCriticalException("File access failed!");
        }
    }
}