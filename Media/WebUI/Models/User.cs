using System;
namespace WebUI.Models
{
	public class User
	{
        public Guid Id { get; set; }
        public Guid AuthUserId { get; set; }
        public string Username { get; set; }
        public string UsernameUnique { get; set; }
        public string Bio { get; set; }
        public string ProfilePicturePath { get; set; }
    }
}

