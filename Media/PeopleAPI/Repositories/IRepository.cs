using System.Linq.Expressions;

namespace PeopleAPI.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //int Count();
        Task<int> Count();
        //TEntity Get(int id);
        Task<TEntity> Get(Guid id);
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

