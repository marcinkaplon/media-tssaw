using PeopleAPI.Models;
using PeopleAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PeopleAPI.Repositories
{
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        private readonly PeopleDbContext _peopleDbContext;

        public UserProfileRepository(PeopleDbContext context) : base(context)
        {
            _peopleDbContext = context;
        }

        public async Task<UserProfile> GetByAuthId(Guid id)
        {
            var userProfile = await _peopleDbContext.UserProfiles.FirstOrDefaultAsync(u => u.AuthUserId == id);

            if (userProfile == null)
            {
                throw new InvalidOperationException($"User with authUserId {id} not found.");
            }

            return userProfile;
        }

        public async Task<UserProfile> GetByUsername(string username)
        {
            var userProfile = await _peopleDbContext.UserProfiles.FirstOrDefaultAsync(u => u.UsernameUnique == username);

            return userProfile;
        }

        public async Task<List<UserProfile>> GetUsersByIds(List<Guid> userIds)
        {
            return await _peopleDbContext.UserProfiles
                .Where(u => userIds.Contains(u.Id))  
                .ToListAsync();
        }

    }
}

