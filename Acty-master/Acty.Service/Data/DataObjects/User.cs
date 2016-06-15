using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Acty
{
    public partial class User : UserBase
    {
        /*
        public User()
        {
            Memberships = new HashSet<Membership>();
        }

        [JsonIgnore]
        public ICollection<Membership> Memberships
        {
            get;
            set;
        }
        */

        [JsonProperty(PropertyName = "ProfilePicture")]
        public UserPicture ProfilePicture
        {
            get;
            set;
        }
    }

    public class DBUser : UserBase
    {
        [JsonProperty(PropertyName = "UserProfilePictureBlob")]
        public string UserProfilePictureBlob
        {
            get;
            set;
        }
    }
}