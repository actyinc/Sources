using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentDBDataService
{
    public class Constants
    {
        // Stories keys
        public const string OwnerIdKey = "OwnerId";
        public const string HeadingKey = "Heading";
        public const string CategoryKey = "Category";
        public const string StatusKey = "Status";
        public const string MessageKey = "Message";
        public const string IsLocalKey = "IsLocal";
        public const string KeyWordsKey = "Keywords";

        // User keys
        public const string DOBKey = "DOB";
        public const string EmailKey = "Email";
        public const string PasswordKey = "Password";
        public const string DisplayNameKey = "DisplayName";
        public const string ZipCodeKey = "ZipCode";
        public const string CountryKey = "Country";
        public const string UserPreferencesKey = "Preferences";

        // Common Keys
        public const string UserIdKey = "UserId";
        public const string DataKey = "Data";
        public const string FileNameKey = "FileName";
        public const string ContentTypeKey = "ContentType";
        public const string ContentLengthKey = "ContentLength";
        public const string CreatedDateKey = "CreatedDate";

        // StorageBD constants
        public const string UserProfilePictureBlobKey = "UserProfilePictureBlob";
    }
}