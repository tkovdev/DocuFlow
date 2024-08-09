using Data;
using DataAccess.Abstractions;
using DataAccess.DAL;

namespace DataAccess.Services;

public class DocumentEntityService : AbstractEntityService<Document>
{
    public DocumentEntityService(IMongoDb mongoDb) : base(mongoDb)
    {
    }
}