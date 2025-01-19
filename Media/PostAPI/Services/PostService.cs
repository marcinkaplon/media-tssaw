using System;
using Microsoft.EntityFrameworkCore;
using PostAPI.Data;
using PostAPI.Models;
using PostAPI.Models.Dto;
using PostAPI.Services;

namespace PostAPI.Services
{
    public class PostService : IPostService
    {
        private readonly IPostUnitOfWork _uow;

        public PostService(IPostUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<Post>> GetAllPostsOfUser(Guid userId)
        {
            var posts = await _uow.PostRepository.GetPostsByUserId(userId);
            posts = posts.OrderByDescending(p => p.CreatedAt).ToList();
            return posts;
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var posts = await _uow.PostRepository.GetAll();
            posts = posts.OrderByDescending(p => p.CreatedAt).ToList();

            return posts;
        }

        public async Task<Post> GetPostById(Guid id)
        {
            var post = await _uow.PostRepository.Get(id);
            if (post is null)
            {
                return null;
            }
            return post;
        }

        public async Task<Post> CreatePost(CreatePostDto postDto)
        {
            Post post = new Post(postDto.UserId, postDto.Content);
            await _uow.PostRepository.Insert(post);
            _uow.Commit();
            return post;
        }

        public async Task<Post> UpdatePost(Guid id, UpdatePostDto post)
        {
            var existingPost = await _uow.PostRepository.Get(id);
            if (existingPost == null)
            {
                return null;
            }

            existingPost.Content = post.Content;

            _uow.Commit();
            return existingPost;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            var post = await _uow.PostRepository.Get(id);
            if (post == null)
            {
                return false;
            }

            await _uow.PostRepository.Delete(post);
            _uow.Commit();
            return true;
        }
    }
}


