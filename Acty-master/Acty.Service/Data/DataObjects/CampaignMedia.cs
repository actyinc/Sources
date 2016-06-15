using System.IO;
using Newtonsoft.Json;

namespace Acty
{
    public class CampaignMedia
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Stream Data { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "contentLength")]
        public long ContentLength { get; set; }
    }
}