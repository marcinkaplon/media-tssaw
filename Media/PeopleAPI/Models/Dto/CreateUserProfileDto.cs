using System;
namespace PeopleAPI.Models.Dto
{
	public class CreateUserProfileDto
	{
        public Guid AuthUserId { get; set; } // foreign key
        public string Username { get; set; }
        public string UsernameUnique { get; set; }
        public string Bio { get; set; }
    }
}

