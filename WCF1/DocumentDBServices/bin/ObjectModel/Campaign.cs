using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentDBDataService
{
    public enum CampaignCategory
    {
        Environmental,
        Social,
        Political,
        Economical,
        Local
    }

    public enum CampaignStatus
    {
        Completed,
        InProgress,
        OnHold,
        Active,
        Suspended,
        Cancelled,
        Flagged
    }

    public class CampaignBase
    {
        public string CreatedDate { get; set; }
        public string OwnerId { get; set; }
        public string Heading { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public bool IsLocal { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string[] KeyWords { get; set; }
        public string LastUpdatedDate { get; set; }
        public int CommentsCount { get; set; }
        public int participationCount { get; set; }
        public List<string> Events { get; set; }
        public string Status { get; set; }
    }

    public class Campaign : CampaignBase
    {
        public CampaignMedia CampaignVisualResource { get; set; }
    }

    public class DBCampaign : CampaignBase
    {
        public string StoryMediaResourceBlob { get; set; }
        public string id { get; set; }
    }
}