using System;
namespace WebUI.Models
{
	public class UserProfileViewModel
	{
		public User User { get; set; }
        public List<Post> Posts { get; set; }
		public bool IsFollowed { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}

