using System;
using PeopleAPI.Models;
using PeopleAPI.Models.Dto;

namespace PeopleAPI.Services
{
	public interface IUserService
	{
        Task<IEnumerable<UserProfile>> GetAllUsers();
        Task<UserProfile> GetUserById(Guid id);
        Task<UserProfile> GetUserByAuthId(Guid id);
        Task<UserProfile> GetUserByUsername(string username);
        Task<UserProfile> CreateUser(CreateUserProfileDto userProfileDto);
        Task<UserProfile> UpdateUser(Guid id, UpdateUserProfileDto userProfile);
        Task<bool> DeleteUser(Guid id);
        Task FollowUser(Guid followerId, Guid followeeId);
        Task UnfollowUser(Guid followerId, Guid followeeId);
        Task<int> GetFollowersCount(Guid userId);
        Task<int> GetFollowingCount(Guid userId);
        Task<bool> DoesUserFollow(string followerName, string followeeName);
        Task<IEnumerable<UserProfile>> GetFollows(Guid userId);
    }
}

