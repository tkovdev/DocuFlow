using Data.Interfaces;
using MongoDB.Driver;
using DataAccess.DAL;
using DataAccess.Exceptions;
using DataAccess.Interfaces;
using MongoDB.Bson;

namespace DataAccess.Abstractions;

/**
 * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
 * <summary>
 * AbstractEntityService implements basic read / write procedures to access IEntity objects stored in MongoDB.
 * This abstract class should be inherited by a concrete class, where the concrete class defines the generic type represented by the IEntity.
 * The concrete class then has access to the standard read/write procedures and can overwrite them or add additional procedures.
 * For example: BookEntityService : AbstractEntityService&lt;Book&gt;
 * </summary>
 */
public abstract class AbstractEntityService<TEntity> : IEntityService<TEntity> where TEntity : IEntity
{
    protected IMongoCollection<TEntity> Collection { get; }

    public IMongoCollection<TEntity> GetCollection()
    {
        return Collection;
    }
    
    /**
     * <param name="mongoDb">An instance of a that contains a database connection</param>
     * <summary>Instantiate a new AbstractEntityService using the IMongoDb interface</summary>
     */
    public AbstractEntityService(IMongoDb mongoDb)
    {
        Collection = mongoDb.Database.GetCollection<TEntity>(typeof(TEntity).Name);
    }
    
    /**
     * <inheritdoc/>
     * <exception cref="DataAccessWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="DataAccessCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Get(ObjectId id)
    {
        var result = await Collection.FindAsync<TEntity>(x => id == x.Id, default, default);
        if (result is not null)
        {
            var x = result.Current;
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new DataAccessWarningException($"Could not find {typeof(TEntity).Name} with Id {id}");
            }
        }
        throw new DataAccessWarningException($"Could not find {typeof(TEntity).Name} with Id {id}");
    }
    
    /**
     * <inheritdoc/>
     * <exception cref="DataAccessWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="DataAccessCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Create(TEntity entity)
    {
        try
        {
            await Collection.InsertOneAsync(entity);
        }
        catch (Exception)
        {
            throw new DataAccessCriticalException($"{typeof(TEntity).Name} creation failed!");
        }
        var result = await Collection.FindAsync(x => entity.Id == x.Id);
        if (result is not null)
        {
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new DataAccessWarningException($"{typeof(TEntity).Name} save successful but does not appear in the database!");
            }
        } 
        throw new DataAccessWarningException($"{typeof(TEntity).Name} save successful but does not appear in the database!");
    }

    /**
     * <inheritdoc/>
     * <exception cref="DataAccessWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="DataAccessCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Update(TEntity entity)
    {
        try
        {
            await Collection.ReplaceOneAsync(x => entity.Id == x.Id, entity);
        }
        catch (Exception)
        {
            throw new DataAccessWarningException($"{typeof(TEntity).Name} update not successful!");
        }
        var result = await Collection.FindAsync(x => entity.Id == x.Id);
        if (result is not null)
        {
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new DataAccessWarningException($"{typeof(TEntity).Name} update successful but does not appear in the database!");
            }
        }
        throw new DataAccessCriticalException($"{typeof(TEntity).Name} update successful but does not appear in the database!");
    }

    /**
     * <inheritdoc/>
     * <exception cref="DataAccessWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="DataAccessCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Delete(ObjectId id)
    {
        var result = await Collection.FindAsync(x => id == x.Id);
        if (result is null) throw new DataAccessWarningException($"{typeof(TEntity).Name} with id {id} not found!");
        try
        {
            await Collection.DeleteOneAsync(x => id == x.Id);
        }
        catch (Exception)
        {
            throw new DataAccessWarningException($"{typeof(TEntity).Name} delete not successful!");
        }
        try
        {
            return await result.FirstAsync();
        }
        catch (Exception)
        {
            throw new DataAccessWarningException($"{typeof(TEntity).Name} delete successful but does not appear in the database!");
        }
    }
}