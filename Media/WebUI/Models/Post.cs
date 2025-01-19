using System;
namespace WebUI.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User Author { get; set; }
    }
}

