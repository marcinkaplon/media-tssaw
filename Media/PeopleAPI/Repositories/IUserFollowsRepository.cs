using System;
using PeopleAPI.Models;

namespace PeopleAPI.Repositories
{
    public interface IUserFollowsRepository : IRepository<UserFollows>
    {
        Task<UserFollows> GetByFollowerFollowee(Guid followerId, Guid followeeId);
        Task<int> GetFollowersCount(Guid followeeId);
        Task<int> GetFollowingCount(Guid followerId);
        Task<List<Guid>> GetFolloweeIds(Guid followerId);
    }
}

