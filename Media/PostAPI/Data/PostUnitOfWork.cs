using System;
using PostAPI.Repositories;

namespace PostAPI.Data
{
    public class PostUnitOfWork : IPostUnitOfWork
    {
        private readonly PostDbContext _context;

        public IPostRepository PostRepository { get; }


        public PostUnitOfWork(
            PostDbContext context,
            IPostRepository postRepository
            )
        {
            this._context = context;
            this.PostRepository = postRepository;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
