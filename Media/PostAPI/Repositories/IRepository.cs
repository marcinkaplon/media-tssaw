using System.Linq.Expressions;

namespace PostAPI.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //int Count();
        Task<int> Count();
        //IList<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAll();
        //IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task<IList<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
        //void Insert(TEntity entity);
        Task Insert(TEntity entity);
        //void Delete(TEntity entity);
        Task Delete(TEntity entity);
    }
}

