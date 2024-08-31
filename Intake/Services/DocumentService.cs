using Data.Access.Abstractions.Exceptions;
using Data.Access.Abstractions.Interfaces;
using Data.Models;
using Grpc.Core;

namespace Intake.Services;

public class DocumentService : DocumentProto.DocumentProtoBase
{
    private readonly ILogger<DocumentService> _logger;
    private readonly IEntityService<Document> _entityService;
    private readonly IFileService _fileService;

    public DocumentService( ILogger<DocumentService> logger, 
                            IEntityService<Document> entityService, 
                            IFileService fileService
                          )
    {
        _logger = logger;
        _entityService = entityService;
        _fileService = fileService;
    }
    
    public override async Task<DocumentReply> Ingest(DocumentRequest request, ServerCallContext context)
    {
        var file = new DocumentFile()
        {
            OriginalName = request.Name,
            Extension = request.Extension,
            Path = String.Empty,
            FileName = Guid.NewGuid().ToString()
        };

        try
        {
            var stream = new MemoryStream(request.Binary.ToByteArray());
            var finalPath = await _fileService.SaveFile(stream, Path.Combine(file.Path, file.FileName));
            file.Path = finalPath;
        }
        catch (Exception e)
        {
            throw MongoRmExceptionFactory.DataAccessCriticalException("File not saved! No data has been stored.");
        }

        try
        {
            var c = await _entityService.Create(new Document
            {
                File = file,
                Signatures = new List<Signature>()
                {
                    new Signature(new User("T", "K", "katie", "P"))
                }
            });

            return await Task.FromResult(new DocumentReply()
            {
                Id = c.Id.ToString()
            });
        }
        catch (Exception e)
        {
            await _fileService.DeleteFile(file.FileName);
            throw MongoRmExceptionFactory.DataAccessCriticalException("Database save failed! No data has been stored.");
        }
    }
}