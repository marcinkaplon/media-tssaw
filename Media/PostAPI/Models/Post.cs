using System;
namespace PostAPI.Models
{
    public class Post
    {
        public Post(Guid userId, string content)
        {
            UserId = userId;
            Content = content;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; } // foreign key
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

