using Data.Access.Abstractions.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Data.Access.DAL;


/**
 * <summary>
 * MongoDb is a concrete class and implements IMongoDb. In most cases, this concrete class will be used for the MongoClient &amp; Database Connection.
 * </summary>
 */
public class MongoDb : IMongoDb
{
    /**
     * <inheritdoc/>
     */
    public IMongoClient Client { get; }
    /**
     * <inheritdoc/>
     */
    public IMongoDatabase Database { get; }

    /**
     * <summary>A MongoDb instance contains a Client and a Database. When instantiated, this object will use appsettings to connect to the database.</summary>
     */
    public MongoDb(IConfiguration configuration)
    {
        var mongoClientSettings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDb"));
        Client = new MongoClient(mongoClientSettings);
        Database = Client.GetDatabase(configuration.GetValue<string>("DatabaseName"));
    }
}