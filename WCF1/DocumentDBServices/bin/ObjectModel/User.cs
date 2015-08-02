using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentDBDataService
{
    public class User
    {
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public UserPicture ProfilePicture { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CreatedDate { get; set; }
        public UserPreferences Preferences { get; set; }
    }

    public class DBUser
    {
        public string id { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string UserProfilePictureBlob { get; set; }
        public string CreatedDate { get; set; }
        public UserPreferences Preferences { get; set; }
    }
}