using System;
using Microsoft.AspNetCore.Mvc;
using PostAPI.Models;
using PostAPI.Models.Dto;
using PostAPI.Services;

namespace PostAPI.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("getAllByUser/{userId}")]
        public async Task<IActionResult> GetAllPostsOfUser(Guid userId)
        {
            var posts = await _postService.GetAllPostsOfUser(userId);
            return Ok(posts);
        }

        [HttpGet("get{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost(CreatePostDto post)
        {
            var createdPost = await _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, UpdatePostDto post)
        {
            var updatedPost = await _postService.UpdatePost(id, post);
            if (updatedPost == null)
            {
                return NotFound();
            }

            return Ok(updatedPost);
        }

        [HttpDelete("remove{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var isDeleted = await _postService.DeletePost(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}


