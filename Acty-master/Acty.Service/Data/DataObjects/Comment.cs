using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Acty
{
    public class Comment : Resource
    {
        [JsonProperty(PropertyName = "createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "campaignId")]
        public string CampaignId { get; set; }

        [JsonProperty(PropertyName = "likedUsers")]
        public List<string> LikedUsers { get; set; }

        [JsonProperty(PropertyName = "dislikedUsers")]
        public List<string> DislikedUsers { get; set; }

        [JsonProperty(PropertyName = "replyCommentId")]
        public string ReplyCommentId { get; set; }

        [JsonProperty(PropertyName = "lastRepliedDate")]
        public string LastRepliedDate { get; set; }

        [JsonProperty(PropertyName = "keyWords")]
        public string[] KeyWords { get; set; }

        [JsonProperty(PropertyName = "replyCount")]
        public int ReplyCount { get; set; }

        public DBComment ToDBComment()
        {
            return new DBComment()
            {
                CreatedDate = this.CreatedDate,
                OwnerId = this.OwnerId,
                Description = this.Description,
                CampaignId = this.CampaignId,
                LikedUsers = this.LikedUsers,
                DislikedUsers = this.DislikedUsers,
                ReplyCommentId = this.ReplyCommentId,
                LastRepliedDate = this.LastRepliedDate,
                KeyWords = this.KeyWords,
                ReplyCount = this.ReplyCount
            };
        }
    }

    public class DBComment : Comment
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}