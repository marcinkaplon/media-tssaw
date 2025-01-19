using System;
namespace WebUI.Models
{
	public class UserEditViewModel
	{
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePicturePath { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}

