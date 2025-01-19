using System;
using Microsoft.EntityFrameworkCore;
using PeopleAPI.Data;
using PeopleAPI.Models;
using PeopleAPI.Models.Dto;
using PeopleAPI.Services;

namespace PeopleAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IPeopleUnitOfWork _uow;

        public UserService(IPeopleUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<UserProfile>> GetAllUsers()
        {
            return await _uow.UserProfileRepository.GetAll();
        }

        public async Task<UserProfile> GetUserById(Guid id)
        {
            var UserProfile = await _uow.UserProfileRepository.Get(id);
            if (UserProfile is null)
            {
                return null;
            }
            return UserProfile;
        }

        public async Task<UserProfile> GetUserByAuthId(Guid id)
        {
            var UserProfile = await _uow.UserProfileRepository.GetByAuthId(id);
            if (UserProfile is null)
            {
                return null;
            }
            return UserProfile;
        }

        public async Task<UserProfile> GetUserByUsername(string username)
        {
            var UserProfile = await _uow.UserProfileRepository.GetByUsername(username);
            if (UserProfile is null)
            {
                return null;
            }
            return UserProfile;
        }

        public async Task<UserProfile> CreateUser(CreateUserProfileDto userProfileDto)
        {
            UserProfile userProfile = new UserProfile(
                userProfileDto.AuthUserId,
                userProfileDto.Username,
                userProfileDto.UsernameUnique,
                userProfileDto.Bio
            );
            await _uow.UserProfileRepository.Insert(userProfile);
            _uow.Commit();
            return userProfile;
        }

        public async Task<UserProfile> UpdateUser(Guid id, UpdateUserProfileDto userProfile)
        {
            var existingUser = await _uow.UserProfileRepository.Get(id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Username = userProfile.Username;
            existingUser.Bio = userProfile.Bio;
            existingUser.ProfilePicturePath = userProfile.ProfilePicturePath;

            _uow.Commit();
            return existingUser;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _uow.UserProfileRepository.Get(id);
            if (user == null)
            {
                return false;
            }

            await _uow.UserProfileRepository.Delete(user);
            _uow.Commit();
            return true;
        }

        public async Task FollowUser(Guid followerId, Guid followeeId)
        {
            var follower = await _uow.UserProfileRepository.Get(followerId);
            var followee = await _uow.UserProfileRepository.Get(followeeId);

            if (follower == null || followee == null || follower.Id == followee.Id)
            {
                throw new ArgumentException("Invalid follower or followee.");
            }

            var existingFollow = await _uow.UserFollowsRepository.GetByFollowerFollowee(followerId, followeeId);
            if (existingFollow != null)
                throw new Exception("You already follow this user.");

            UserFollows newUserFollows = new UserFollows(
                followerId,
                followeeId
            );

            await _uow.UserFollowsRepository.Insert(newUserFollows);
            _uow.Commit();
        }

        public async Task UnfollowUser(Guid followerId, Guid followeeId)
        {
            var follower = await _uow.UserProfileRepository.Get(followerId);
            var followee = await _uow.UserProfileRepository.Get(followeeId);

            if (follower == null || followee == null || follower.Id == followee.Id)
            {
                throw new ArgumentException("Invalid follower or followee.");
            }

            var existingFollow = await _uow.UserFollowsRepository.GetByFollowerFollowee(followerId, followeeId);
            if (existingFollow == null)
                throw new Exception("This user is not followed by this followee.");

            await _uow.UserFollowsRepository.Delete(existingFollow);
            _uow.Commit();
        }

        public async Task<int> GetFollowersCount(Guid userId)
        {
            int following_count = await _uow.UserFollowsRepository.GetFollowersCount(userId);
            return following_count;
        }

        public async Task<int> GetFollowingCount(Guid userId)
        {
            int following_count = await _uow.UserFollowsRepository.GetFollowingCount(userId);
            return following_count;
        }

        public async Task<bool> DoesUserFollow(string followerName, string followeeName)
        {
            var follower = await _uow.UserProfileRepository.GetByUsername(followerName);
            var followee = await _uow.UserProfileRepository.GetByUsername(followeeName);

            if (follower == null || followee == null)
            {
                throw new ArgumentException("Invalid follower or followee.");
            }

            var existingFollow = await _uow.UserFollowsRepository.GetByFollowerFollowee(follower.Id, followee.Id);
            if (existingFollow != null)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<UserProfile>> GetFollows(Guid userId)
        {
            List<Guid> followeeIds = await _uow.UserFollowsRepository.GetFolloweeIds(userId);
            List<UserProfile> followedUsers = await _uow.UserProfileRepository.GetUsersByIds(followeeIds);
            return followedUsers;
        }
    }
}


