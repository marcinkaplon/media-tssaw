using PeopleAPI.Repositories;

namespace PeopleAPI.Data
{
    public interface IPeopleUnitOfWork : IDisposable
    {
        IUserProfileRepository UserProfileRepository { get; }
        IUserFollowsRepository UserFollowsRepository { get; }

        void Commit();
    }
}

