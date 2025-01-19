using PostAPI.Models;
using PostAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PostAPI.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly PostDbContext _postDbContext;

        public PostRepository(PostDbContext context) : base(context)
        {
            _postDbContext = context;
        }

        public async Task<List<Post>> GetPostsByUserId(Guid userId)
        {
            return await _postDbContext.Posts
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Post> Get(Guid id)
        {
            return await _postDbContext.Set<Post>().FindAsync(id);
        }
    }
}

