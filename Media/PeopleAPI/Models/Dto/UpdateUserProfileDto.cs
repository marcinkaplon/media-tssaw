using System;
namespace PeopleAPI.Models.Dto
{
	public class UpdateUserProfileDto
	{
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePicturePath { get; set; }
    }
}

