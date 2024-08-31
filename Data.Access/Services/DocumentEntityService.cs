using Data.Models;
using Data.Access.Abstractions;
using Data.Access.Abstractions.Interfaces;

namespace Data.Access.Services;

public class DocumentEntityService : AbstractEntityService<Document>
{
    public DocumentEntityService(IMongoDb mongoDb) : base(mongoDb)
    {
    }
}