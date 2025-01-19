using System;

namespace PostAPI.Models.Dto
{
    public class CreatePostDto
    {
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}



