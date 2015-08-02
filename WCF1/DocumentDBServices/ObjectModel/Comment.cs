using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentDBDataService
{
    public class Comment
    {
        public string CreatedDate { get; set; }
        public string OwnerId { get; set; }
        public string Description { get; set; }
        public string CampaignId { get; set; }
        public List<string> LikedUsers { get; set; }
        public List<string> DislikedUsers { get; set; }
        public string ReplyCommentId { get; set; }
        public string LastRepliedDate { get; set; }
        public string[] KeyWords { get; set; }
        public int ReplyCount { get; set; }
    }

    public class DBComment : Comment
    {
        public string id { get; set; }
    }
}