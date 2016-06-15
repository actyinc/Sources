using System.Collections.Generic;
using Newtonsoft.Json;

namespace Acty
{
    public class Event
    {
        [JsonProperty(PropertyName = "createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty(PropertyName = "lastUpdatedDate")]
        public string LastUpdatedDate { get; set; }

        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty(PropertyName = "campaignId")]
        public string CampaignId { get; set; }

        [JsonProperty(PropertyName = "followers")]
        public List<string> Followers { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public List<string> Comments { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "keyWords")]
        public string[] KeyWords { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        // This could be something like link to google/outlook calender event.
        // Google hangout link.
        [JsonProperty(PropertyName = "externalEventLink")]
        public string ExternalEventLink { get; set; }

        public DBEvent ToDBEvent()
        {
            return new DBEvent()
            {
                CreatedDate = this.CreatedDate,
                OwnerId = this.OwnerId,
                Description = this.Description,
                CampaignId = this.CampaignId,
                LastUpdatedDate = this.LastUpdatedDate,
                Followers = this.Followers,
                Comments = this.Comments,
                Time = this.Time,
                KeyWords = this.KeyWords,
                ZipCode = this.ZipCode,
                Country = this.Country,
                Location = this.Location
            };
        }
    }

    public class DBEvent : Event
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}