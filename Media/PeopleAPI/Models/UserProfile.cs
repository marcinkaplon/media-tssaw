using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PeopleAPI.Models
{
    public class UserProfile
    {
        public UserProfile(Guid authUserId, string username, string usernameUnique, string bio)
        {
            //Id = id;
            AuthUserId = authUserId;
            UsernameUnique = usernameUnique;
            Username = username;
            Bio = bio;
        }
        public Guid Id { get; set; }
        public Guid AuthUserId { get; set; } // foreign key
        public string Username { get; set; }
        public string UsernameUnique { get; set; }
        public string Bio { get; set; }
        public string ProfilePicturePath { get; set; } = "/images/profiles/default_pic.jpg";
        [JsonIgnore]
        public virtual ICollection<UserFollows> Followers { get; set; } = new List<UserFollows>();
        [JsonIgnore]
        public virtual ICollection<UserFollows> Following { get; set; } = new List<UserFollows>();
    }
}

