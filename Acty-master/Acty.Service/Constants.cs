using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acty.Service
{
    internal class Constants
    {
        internal static readonly string HubConnectionString = "Endpoint=sb://actynotificationhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8d4S3T9aTUFKAC1X7B/6EAzuw/LgIcde9gCoT+I8Rxs=";
        internal static readonly string HubName = "ActyNotificationHub";

        // Stories keys
        public const string OwnerIdKey = "OwnerId";
        public const string HeadingKey = "Heading";
        public const string CategoryKey = "Category";
        public const string StatusKey = "Status";
        public const string MinAgeKey = "MinAge";
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
