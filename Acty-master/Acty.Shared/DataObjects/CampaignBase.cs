using System.Collections.Generic;
using Newtonsoft.Json;

namespace Acty
{
    public partial class CampaignBase
    {
        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "OwnerId")]
        public string OwnerId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Heading")]
        public string Heading
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Category")]
        public string Category
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Message")]
        public string Message
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "IsLocal")]
        public bool IsLocal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "ZipCode")]
        public string ZipCode
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Country")]
        public string Country
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "KeyWords")]
        public string[] KeyWords
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "LastUpdatedDate")]
        public string LastUpdatedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "CommentsCount")]
        public int CommentsCount
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "ParticipationCount")]
        public int participationCount
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Events")]
        public List<string> Events
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Status")]
        public string Status
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "MinAge")]
        public int MinAge
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "ContentName")]
        public string ContentName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "ContentType")]
        public string ContentType
        {
            get;
            set;
        }
    }
}