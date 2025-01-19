using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAPI.Models;
using PeopleAPI.Models.Dto;
using PeopleAPI.Services;

namespace PeopleAPI.Controllers
{
    [Route("user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("get{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("getByAuthId/{authUserId}")]
        public async Task<IActionResult> GetUserByAuthId(Guid authUserId)
        {
            var user = await _userService.GetUserByAuthId(authUserId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("getByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserProfileDto userProfile)
        {
            var createdUser = await _userService.CreateUser(userProfile);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserProfileDto userProfile)
        {
            var updatedUser = await _userService.UpdateUser(id, userProfile);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("remove{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var isDeleted = await _userService.DeleteUser(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("follow/{followerId}/{followeeId}")]
        public async Task<IActionResult> FollowUser(Guid followerId, Guid followeeId)
        {
            try
            {
                await _userService.FollowUser(followerId, followeeId);
                return Ok("Followed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unfollow/{followerId}/{followeeId}")]
        public async Task<IActionResult> UnfollowUser(Guid followerId, Guid followeeId)
        {
            try
            {
                await _userService.UnfollowUser(followerId, followeeId);
                return Ok("Unfollowed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getFollowerCount/{userId}")]
        public async Task<IActionResult> GetFollowersCount(Guid userId)
        {
            var count = await _userService.GetFollowersCount(userId);
            return Ok(new { ResultCount = count });
        }

        [HttpGet("getFollowingCount/{userId}")]
        public async Task<IActionResult> GetFollowingCount(Guid userId)
        {
            var count = await _userService.GetFollowingCount(userId);
            return Ok(new { ResultCount = count });
        }

        [HttpGet("doesUserFollow/{followerName}/{followeeName}")]
        public async Task<IActionResult> DoesUserFollow(string followerName, string followeeName)
        {
            try
            {
                var result = await _userService.DoesUserFollow(followerName, followeeName);
                return Ok(new { DoesFollow = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getFollows/{id}")]
        public async Task<IActionResult> GetFollows(Guid id)
        {
            try
            {
                var result = await _userService.GetFollows(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


