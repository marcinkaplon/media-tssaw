using PeopleAPI.Models;

namespace PeopleAPI.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetByAuthId(Guid id);
        Task<UserProfile> GetByUsername(string username);
        Task<List<UserProfile>> GetUsersByIds(List<Guid> userIds);
    }
}
