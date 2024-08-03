using System.Linq.Expressions;

namespace HealthMed.Domain.Interfaces.Repositories;

public interface IBaseRepository<T, G> where T : class
{
    Task InsertAsync(T obj);
    Task UpdateAsync(T obj);
    Task DeleteAsync(G id);
    Task<IList<T>> SelectAsync(Expression<Func<T, bool>> predicate);
    Task<IList<T>> SelectAsync();
    Task<T> SelectAsync(G id);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);
}