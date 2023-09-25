using MongoDB.Driver;
using System.Linq.Expressions;

namespace Repository.Service
{
    public class Repository<T> : IRepository<T>
    {
        private readonly IMongoCollection<T> _collection;
        public Repository(IMongoClient client, string dataBaseName, string collectionName)
        {
            var database = client.GetDatabase(dataBaseName);
            _collection = database.GetCollection<T>(collectionName);
        }
        public async Task<T> addOneItem(T item)
        {
            try
            {
                await _collection.InsertOneAsync(item);
                return item;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public async Task<List<T>> addManyItem(List<T> items)
        {
            try
            {
                await _collection.InsertManyAsync(items);
                return items;
            }catch (Exception ex)
            {
                return new List<T>();
            }
        }
        public async Task<IEnumerable<T>> getAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetsByFilterAsync(Expression<Func<T, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }
        public async Task<bool> removeItemByValue(object value)
        {
            var filter = Builders<T>.Filter.Eq("_id", value); // Assuming "ID" is the field in your MongoDB documents
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        public async Task<T> updateItemByValue(object value, UpdateDefinition<T> updateDefinition)
        {
            var filter = Builders<T>.Filter.Eq("_id", value);
            var updateResult = await _collection.UpdateOneAsync(filter, updateDefinition);
            if (updateResult.ModifiedCount > 0)
            {
                T updatedItem = await _collection.Find(filter).FirstOrDefaultAsync();
                return updatedItem;
            }
            else
            {
                return default(T);
            }
        }
    }
}
