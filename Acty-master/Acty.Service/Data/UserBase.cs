using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Acty
{
    public partial class UserBase : Resource
    {
        [JsonProperty(PropertyName = "DisplayName")]
        public string DisplayName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "DOB")]
        public string DOB
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Email")]
        public string Email
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Password")]
        public string Password
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

        [JsonProperty(PropertyName = "CreatedDate")]
        public string CreatedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Preferences")]
        public UserPreferences Preferences
        {
            get;
            set;
        }
    }
}