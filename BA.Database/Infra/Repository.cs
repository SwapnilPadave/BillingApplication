using BA.Entities.Infra;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BA.Database.Infra
{
    public class Repository<T>(BAContext context) : IRepository<T> where T : class
    {
        private readonly BAContext _context = context;
        public T Add(T entity)
        {
            _context.Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> GetAsync(Guid guid) => await _context.Set<T>().FindAsync(guid);

        public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            return true;
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _context.AddRangeAsync(entities);
            await Task.CompletedTask;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();

        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            _context.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return await Task.FromResult(orderBy != null ? orderBy(query) : query);

        }

        public virtual IQueryable<T> Query(bool eager = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (eager)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }


        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            return await Query(eager).FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            return await Query(eager).Where(predicate).AnyAsync();
        }

        public virtual IQueryable<T> GetAllAsQuerable(string navigationPropertyInclude1, string navigationPropertyInclude2)
        {
            return _context.Set<T>().Include(navigationPropertyInclude1).Include(navigationPropertyInclude2).AsQueryable();
        }

        public virtual IQueryable<T> GetAllAsQuerable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            var dbSet = _context.Set<T>();
            return Query(eager).Where(predicate).AsQueryable();
        }

        public virtual IQueryable<T> FindWithChilds(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            var dbSet = _context.Set<T>();
            var asQuery = Query(eager).Where(predicate).AsQueryable();

            Func<IQueryable<T>, IQueryable<T>> includeFunc = f => f;
            foreach (var prop in typeof(T).GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(IncludeAttribute))))
            {
                Func<IQueryable<T>, IQueryable<T>> chainedIncludeFunc = f => f.Include(prop.Name);
                includeFunc = Compose(includeFunc, chainedIncludeFunc);
            }
            return includeFunc(asQuery);
        }

        private static Func<T, T> Compose<T>(Func<T, T> innerFunc, Func<T, T> outerFunc)
        {
            return arg => outerFunc(innerFunc(arg));
        }


        //https://github.com/thepirat000/Audit
        //.NET/issues/53
        public async Task AttachUpdateEntityAsync(T entity)
        {
            var entry = _context.Set<T>().Attach(entity); // Attach to the DbContext
            var updated = entry.CurrentValues.Clone();
            entry.Reload();
            entry.CurrentValues.SetValues(updated);
            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
