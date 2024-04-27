using MongoDB.Driver;

namespace DataAccess.DAL;

/**
 * <summary>
 * IMongoDb is an interface for MongoDb. The primary purpose of this interface is for decoupling. In most cases, the matching concrete class MongoDb can be used.
 * </summary>
 */
public interface IMongoDb
{
    /**
     * <summary>Client is an instance of IMongoClient. Use this property to access the underlying MongoDb connection</summary>
     * <seealso cref="MongoDB.Driver.IMongoClient"/>
     * <seealso cref="MongoDB.Driver.MongoClient"/>
     */
    public IMongoClient Client { get; }
    
    /**
     * <summary>Database is an instance of IMongoDatabase. Use this property to access the Mongo Database</summary>
     * <seealso cref="MongoDB.Driver.IMongoDatabase"/>
     */
    public IMongoDatabase Database { get; }
}