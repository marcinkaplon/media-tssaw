using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.Dto
{
    public class DeleteAccountDto
    {
        public DeleteAccountDto(string email, string password, string confirmPassword)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

