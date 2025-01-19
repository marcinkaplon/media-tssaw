using System;
using PeopleAPI.Models;
using PeopleAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PeopleAPI.Repositories
{
    public class UserFollowsRepository : Repository<UserFollows>, IUserFollowsRepository
    {
        private readonly PeopleDbContext _peopleDbContext;

        public UserFollowsRepository(PeopleDbContext context) : base(context)
        {
            _peopleDbContext = context;
        }

        public async Task<UserFollows> GetByFollowerFollowee(Guid followerId, Guid followeeId)
        {
            var userFollows = await _peopleDbContext.UserFollows.FirstOrDefaultAsync(u => u.FollowerId == followerId && u.FolloweeId == followeeId);

            return userFollows;
        }

        public async Task<int> GetFollowersCount(Guid followeeId)
        {
            return await _peopleDbContext.UserFollows
                .Where(u => u.FolloweeId == followeeId)
                .CountAsync();
        }

        public async Task<int> GetFollowingCount(Guid followerId)
        {
            return await _peopleDbContext.UserFollows
                .Where(u => u.FollowerId == followerId)
                .CountAsync();
        }

        public async Task<List<Guid>> GetFolloweeIds(Guid followerId)
        {
            return await _peopleDbContext.UserFollows
                .Where(u => u.FollowerId == followerId)
                .Select(u => u.FolloweeId)
                .ToListAsync();
        }

    }
}

