using Data;
using DataAccess.Abstractions;
using DataAccess.DAL;
using DataAccess.Interfaces;
using Grpc.Core;

namespace Intake.Services;

public class DocumentService : DocumentProto.DocumentProtoBase
{
    private readonly ILogger<DocumentService> _logger;
    private readonly IEntityService<Document> _entityService;
    private readonly IConfiguration _configuration;

    public DocumentService(ILogger<DocumentService> logger, IMongoDb mongoDb, IConfiguration configuration)
    {
        _logger = logger;
        _entityService = new AbstractEntityService<Document>(mongoDb);
        _configuration = configuration;
    }
    
    public override async Task<DocumentReply> Ingest(DocumentRequest request, ServerCallContext context)
    {
        var location = _configuration.GetValue<string>("FileLocation");
        var c = await _entityService.Create(new Document
        {
            Location = location,
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
}