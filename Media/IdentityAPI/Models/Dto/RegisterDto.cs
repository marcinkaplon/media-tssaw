using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.Dto
{
	public class RegisterDto
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}

