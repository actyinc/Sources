using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentDBDataService
{
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