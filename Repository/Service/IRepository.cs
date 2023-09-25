using MongoDB.Driver;
using System.Linq.Expressions;

namespace Repository.Service
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> getAllAsync();
        Task<T> addOneItem(T item);
        Task<List<T>> addManyItem(List<T> items);
        Task<bool> removeItemByValue(object value);
        Task<T> updateItemByValue(object value, UpdateDefinition<T> updateDefinition);
        Task<IEnumerable<T>> GetsByFilterAsync(Expression<Func<T, bool>> filterExpression);
    }
}
