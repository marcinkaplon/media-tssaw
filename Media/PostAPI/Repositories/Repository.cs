using Microsoft.EntityFrameworkCore;
using PostAPI.Repositories;
using System.Linq.Expressions;

namespace PostAPI.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> Count()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await _context.Set<TEntity>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Insert(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }
    }
}


