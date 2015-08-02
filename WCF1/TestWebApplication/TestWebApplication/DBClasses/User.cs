using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApplication
{
    public class User
    {
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CreatedDate { get; set; }
        public UserPreferences Preferences { get; set; }
    }

    public class DBUser
    {
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

    public class UserPreferences
    {
        // List of locales (countryCode/zipCodes combination) the user is interested in.
        public List<string> InterestedLocales { get; set; }

        public void AddInterestedLocale(string countryCode, string zipCode)
        {
            InterestedLocales.Add(countryCode + "/" + zipCode);
        }

        public void RemoveInterestedLocale(string countryCode, string zipCode)
        {
            InterestedLocales.Remove(countryCode + "/" + zipCode);
        }
    }
}