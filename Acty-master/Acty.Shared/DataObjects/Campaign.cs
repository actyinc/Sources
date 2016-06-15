using Newtonsoft.Json;

namespace Acty
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

    public class Campaign : CampaignBase
    {
        [JsonProperty(PropertyName = "CampaignVisualResource")]
        public CampaignMedia CampaignVisualResource { get; set; }
    }

    public class DBCampaign : CampaignBase
    {
        [JsonProperty(PropertyName = "StoryMediaResourceBlob")]
        public string StoryMediaResourceBlob
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}