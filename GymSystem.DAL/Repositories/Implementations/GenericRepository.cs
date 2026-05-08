using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymSystem.DAL.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
            => await ApplyIncludes(_dbSet, includes).FirstOrDefaultAsync(BuildIdPredicate(id));
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
        public void Update(T entity) => _dbSet.Update(entity);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
            => await ApplyIncludes(_dbSet, includes).Where(predicate).ToListAsync();
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        // --- Helpers ---

        private static IQueryable<T> ApplyIncludes(
            IQueryable<T> query,
            Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
                query = query.Include(include);
            return query;
        }

        private static Expression<Func<T, bool>> BuildIdPredicate(int id)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var prop = Expression.Property(param, "Id");
            var val = Expression.Constant(id);
            var eq = Expression.Equal(prop, val);
            return Expression.Lambda<Func<T, bool>>(eq, param);
        }

    }
}
