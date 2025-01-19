using PostAPI.Repositories;

namespace PostAPI.Data
{
    public interface IPostUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }

        void Commit();
    }
}

