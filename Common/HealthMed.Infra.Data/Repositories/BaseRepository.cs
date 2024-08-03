using HealthMed.Domain.Interfaces.Entities;
using HealthMed.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HealthMed.Infra.Data.Repositories;

public abstract class BaseRepository<TObject, G, TContext> : IBaseRepository<TObject, G>
   where TObject : class, IEntity<G>
   where TContext : DbContext
{
    protected TContext _dataContext;

    public BaseRepository(TContext context)
    {
        _dataContext = context;
    }

    public async Task InsertAsync(TObject obj)
    {
        await _dataContext.Set<TObject>().AddAsync(obj);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TObject obj)
    {
        _dataContext.Entry(obj).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(G id)
    {
        _dataContext.Set<TObject>().Remove(await SelectAsync(id));
        await _dataContext.SaveChangesAsync();
    }

    public async Task<IList<TObject>> SelectAsync(Expression<Func<TObject, bool>> predicate) =>
        await _dataContext.Set<TObject>().Where(predicate).ToListAsync();

    public async Task<IList<TObject>> SelectAsync() =>
        await _dataContext.Set<TObject>().ToListAsync();

    public async Task<TObject> SelectAsync(G id) =>
        await _dataContext.Set<TObject>().FindAsync(id);

    public async Task<TObject> FirstOrDefaultAsync(Expression<Func<TObject, bool>> predicate) =>
        await _dataContext.Set<TObject>().FirstOrDefaultAsync(predicate);

    public async Task<bool> AnyAsync(Expression<Func<TObject, bool>> predicate) =>
        await _dataContext.Set<TObject>().AnyAsync(predicate);

    public IQueryable<TObject> GetQueryable(Expression<Func<TObject, bool>> predicate)
    {
        return _dataContext.Set<TObject>().Where(predicate);
    }
}
