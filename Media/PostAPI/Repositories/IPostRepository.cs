using PostAPI.Models;

namespace PostAPI.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Post> Get(Guid id);
        Task<List<Post>> GetPostsByUserId(Guid userId);
    }
}

