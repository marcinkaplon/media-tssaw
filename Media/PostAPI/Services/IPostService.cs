using System;
using PostAPI.Models;
using PostAPI.Models.Dto;

namespace PostAPI.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<IEnumerable<Post>> GetAllPostsOfUser(Guid userId);
        Task<Post> GetPostById(Guid id);
        Task<Post> CreatePost(CreatePostDto postDto);
        Task<Post> UpdatePost(Guid id, UpdatePostDto post);
        Task<bool> DeletePost(Guid id);
    }
}



