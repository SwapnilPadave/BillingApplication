using System.Linq.Expressions;

namespace BA.Database.Infra
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        T Add(T entity);
        Task<T> GetAsync(Guid guid);
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        T Update(T entity);
        bool Remove(T entity);

        Task AddRangeAsync(List<T> entities);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!);
        IQueryable<T> GetAllAsQuerable(string navigationPropertyInclude1, string navigationPropertyInclude2);
        IQueryable<T> GetAllAsQuerable();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool eager = false);
        IQueryable<T> FindWithChilds(Expression<Func<T, bool>> predicate, bool eager = false);
        Task AttachUpdateEntityAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);
        IQueryable<T> Query(bool eager = false);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool eager = false);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}
