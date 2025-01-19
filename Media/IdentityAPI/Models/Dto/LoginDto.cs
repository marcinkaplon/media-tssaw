using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.Dto
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

