using System;
using System.Text.Json.Serialization;

namespace PeopleAPI.Models
{
	public class UserFollows
	{
        public UserFollows(Guid followerId, Guid followeeId)
        {
            FollowerId = followerId;
            FolloweeId = followeeId;
        }

        public Guid Id { get; set; }
        public Guid FollowerId { get; set; }
        [JsonIgnore]
        public UserProfile Follower { get; set; }

        public Guid FolloweeId { get; set; }
        [JsonIgnore]
        public UserProfile Followee { get; set; }
    }
}

