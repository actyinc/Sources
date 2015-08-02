using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using DocumentDBDataService;

namespace DocumentDBDataService.DocumentDB
{
    public class DocumentDBConnector
    {
        private static string _databaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private static string _usersCollectionId = ConfigurationManager.AppSettings["UsersCollectionId"];
        private static string _campaignsCollectionId = ConfigurationManager.AppSettings["CampaignsCollectionId"];
        private static string _commentsCollectionId = ConfigurationManager.AppSettings["CommentsCollectionId"];
        private static string _eventsCollectionId = ConfigurationManager.AppSettings["EventsCollectionId"];
        private static string _usersStorageUrl = ConfigurationManager.AppSettings["UsersStorageUrl"];
        private static string _campaignsStorageUrl = ConfigurationManager.AppSettings["CampaignsStorageUrl"];

        //This property establishes a new connection to DocumentDB the first time it is used, 
        //and then reuses this instance for the duration of the application avoiding the
        //overhead of instantiating a new instance of DocumentClient with each request
        private static DocumentClient _client;
        private static DocumentClient Client
        {
            get
            {
                if (_client == null)
                {
                    string endpoint = ConfigurationManager.AppSettings["EndPointUrl"];
                    string authKey = ConfigurationManager.AppSettings["AuthorizationKey"];
                    Uri endpointUri = new Uri(endpoint);
                    _client = new DocumentClient(endpointUri, authKey);
                }

                return _client;
            }
        }

        //Use the Database if it exists, if not create a new Database
        private static Database ReadOrCreateDatabase()
        {
            var db = Client.CreateDatabaseQuery()
                            .Where(d => d.Id == _databaseId)
                            .AsEnumerable()
                            .FirstOrDefault();

            if (db == null)
            {
                db = Client.CreateDatabaseAsync(new Database { Id = _databaseId }).Result;
            }

            return db;
        }

        //Use the ReadOrCreateDatabase function to get a reference to the database.
        private static Database _database;
        private static Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = ReadOrCreateDatabase();
                }

                return _database;
            }
        }

        //Use the DocumentCollection if it exists, if not create a new Collection
        private static DocumentCollection ReadOrCreateCollection(string databaseLink, string collectionId)
        {
            DocumentCollection col = Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(c => c.Id == collectionId)
                              .AsEnumerable()
                              .FirstOrDefault();

            if (col == null)
            {
                var collectionSpec = new DocumentCollection { Id = collectionId };
                var requestOptions = new RequestOptions { OfferType = "S1" };

                col = Client.CreateDocumentCollectionAsync(databaseLink, collectionSpec, requestOptions).Result;
            }

            return col;
        }

        //Use the ReadOrCreateCollection function to get a reference to the collection.
        private static DocumentCollection _usersCollection;
        private static DocumentCollection UsersCollection
        {
            get
            {
                if (_usersCollection == null)
                {
                    _usersCollection = ReadOrCreateCollection(Database.SelfLink, _usersCollectionId);
                }

                return _usersCollection;
            }
        }

        //Use the ReadOrCreateCollection function to get a reference to the collection.
        private static DocumentCollection _campaignsCollection;
        private static DocumentCollection CampaignCollection
        {
            get
            {
                if (_campaignsCollection == null)
                {
                    _campaignsCollection = ReadOrCreateCollection(Database.SelfLink, _campaignsCollectionId);
                }

                return _campaignsCollection;
            }
        }

        //Use the ReadOrCreateCollection function to get a reference to the collection.
        private static DocumentCollection _commentsCollection;
        private static DocumentCollection CommentsCollection
        {
            get
            {
                if (_commentsCollection == null)
                {
                    _commentsCollection = ReadOrCreateCollection(Database.SelfLink, _commentsCollectionId);
                }

                return _commentsCollection;
            }
        }

        //Use the ReadOrCreateCollection function to get a reference to the collection.
        private static DocumentCollection _eventsCollection;
        private static DocumentCollection EventsCollection
        {
            get
            {
                if (_eventsCollection == null)
                {
                    _eventsCollection = ReadOrCreateCollection(Database.SelfLink, _eventsCollectionId);
                }

                return _eventsCollection;
            }
        }

        public static bool IsUserNameAvailable(string userName)
        {
            var userNames = Client.CreateDocumentQuery<User>(UsersCollection.DocumentsLink).Where(user => user.Email == userName).AsEnumerable();
            var retVal = userNames.Count() == 0;
            return retVal;
        }

        public static async Task<bool> AddUser(User user)
        {
            var userPic = user.ProfilePicture;
            string uniqueFileName = "";
            if (userPic != null)
            {
                uniqueFileName = userPic.FileName + "_" + Guid.NewGuid();
                await StorageConnector.UploadUserData(uniqueFileName, userPic.ContentType, userPic.Data);
            }

            DBUser dbUser = new DBUser()
            {
                Country = user.Country,
                DisplayName = user.DisplayName,
                DOB = user.DOB,
                Email = user.Email,
                Password = user.Password,
                ZipCode = user.ZipCode,
                UserProfilePictureBlob = uniqueFileName,
                CreatedDate = user.CreatedDate,
                Preferences = user.Preferences
            };
            var createdUser = await Client.CreateDocumentAsync(UsersCollection.DocumentsLink, dbUser);

            if (createdUser == null)
                return false;

            return true;
        }

        public static async Task<bool> UpdateUserProfilePicture(UserPicture userPic)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", userPic.UserId));
            Document userDoc = Client.CreateDocumentQuery(UsersCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            if (userDoc == null)
                return false;

            try
            {
                // Delete the previous profile picture if its already present.
                var blobKey = userDoc.GetPropertyValue<string>(Constants.UserProfilePictureBlobKey);
                if (!string.IsNullOrEmpty(blobKey))
                    await StorageConnector.DeleteUserProfilePicture(blobKey);

                string uniqueFileName = userPic.FileName + "_" + Guid.NewGuid();
                await StorageConnector.UploadUserData(uniqueFileName, userPic.ContentType, userPic.Data);
                userDoc.SetPropertyValue(Constants.UserProfilePictureBlobKey, uniqueFileName);
                await Client.ReplaceDocumentAsync(userDoc.SelfLink, userDoc);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // If isAdd is false, its considered as remove.
        public static async Task<bool> UpdateUserPreference(User user, bool isAdd)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", user.Email));
            Document doc = Client.CreateDocumentQuery(UsersCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            if (doc == null)
                return false;

            DBUser userDoc;
            if ((userDoc = JsonConvert.DeserializeObject<DBUser>(doc.ToString())) == null)
                return false;

            try
            {
                if (isAdd)
                    userDoc.Preferences.InterestedLocales.AddRange(user.Preferences.InterestedLocales);
                else
                    foreach (var item in user.Preferences.InterestedLocales)
                        userDoc.Preferences.InterestedLocales.Remove(item);

                doc.SetPropertyValue(Constants.UserPreferencesKey, userDoc.Preferences);
                await Client.ReplaceDocumentAsync(doc.SelfLink, doc);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> CreateCampaignInDB(Campaign campaign)
        {
            var campaignMedia = campaign.CampaignVisualResource;
            if (campaignMedia == null || campaignMedia.Data == null)
                return false;

            string uniqueFileName = campaignMedia.FileName + "_" + Guid.NewGuid();
            await StorageConnector.UploadStory(uniqueFileName, campaignMedia.ContentType, campaignMedia.Data);

            DBCampaign dbStory = new DBCampaign()
            {
                Country = campaign.Country,
                ZipCode = campaign.ZipCode,
                Category = campaign.Category,
                Status = campaign.Status,
                CreatedDate = campaign.CreatedDate,
                Heading = campaign.Heading,
                IsLocal = campaign.IsLocal,
                KeyWords = campaign.KeyWords,
                Message = campaign.Message,
                OwnerId = campaign.OwnerId,
                StoryMediaResourceBlob = uniqueFileName
            };

            var createdStory = await Client.CreateDocumentAsync(CampaignCollection.DocumentsLink, dbStory);
            if (createdStory == null)
                return false;

            return true;
        }

        public static DBUser GetUser(string userId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", userId));
            DBUser userDoc = Client.CreateDocumentQuery<DBUser>(UsersCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            string blobLink = userDoc.UserProfilePictureBlob;
            if (!string.IsNullOrEmpty(blobLink) && !string.IsNullOrWhiteSpace(blobLink))
                userDoc.UserProfilePictureBlob = _usersStorageUrl + blobLink;

            return userDoc;
        }

        public static IEnumerable<DBCampaign> GetCampaignsForUser(string userId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.OwnerId = \"{0}\") ", userId));
            List<DBCampaign> campaigns = Client.CreateDocumentQuery<DBCampaign>(CampaignCollection.DocumentsLink, querySpec).AsEnumerable().ToList();

            foreach (var campaign in campaigns)
            {
                string blobLink = campaign.StoryMediaResourceBlob;
                if (!string.IsNullOrEmpty(blobLink) && !string.IsNullOrWhiteSpace(blobLink))
                    campaign.StoryMediaResourceBlob = _campaignsStorageUrl + blobLink;
            }

            return campaigns;
        }

        internal static IEnumerable<DBCampaign> GetTopFeedsForUser(string userName)
        {
            DBUser user = GetUser(userName);
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.ZipCode = \"{0}\") ", user.ZipCode));
            List<DBCampaign> campaigns = Client.CreateDocumentQuery<DBCampaign>(CampaignCollection.DocumentsLink, querySpec).AsEnumerable().ToList();

            foreach (var campaign in campaigns)
            {
                string blobLink = campaign.StoryMediaResourceBlob;
                if (!string.IsNullOrEmpty(blobLink) && !string.IsNullOrWhiteSpace(blobLink))
                    campaign.StoryMediaResourceBlob = _campaignsStorageUrl + blobLink;
            }

            return campaigns;
        }

        internal static async Task<bool> AddComment(Comment comment)
        {
            var createdComment = await Client.CreateDocumentAsync(CommentsCollection.DocumentsLink, comment);

            // Currently this just adds the comment. but in reality, the moment you add the comment correcponding 
            // Campaign's comment count and the replyComment count (is this is a reply to a comment) should be incremented.
            // this can be accomplished by stored procedures which the documentDB supports. Need to investigate on that.
            if (createdComment == null)
                return false;

            return true;
        }

        internal static async Task<bool> DeleteComment(string commentId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", commentId));
            Document commentDoc = Client.CreateDocumentQuery(CommentsCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            // Similarly delete Comment Should update all the comment count in events and delete all the replies to the comment.
            if (commentDoc == null)
                return false;

            try
            {
                await Client.DeleteDocumentAsync(commentDoc.SelfLink);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static async Task<bool> AddEvent(Event evt)
        {
            var createdComment = await Client.CreateDocumentAsync(EventsCollection.DocumentsLink, evt);

            if (createdComment == null)
                return false;

            return true;
        }

        internal static async Task<bool> UpdateEvent(DBEvent evt)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.CampaignId = \"{0}\") ", evt.CampaignId));
            Document eventDoc = Client.CreateDocumentQuery(EventsCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null)
                return false;

            try
            {
                // We need to pass in DBEvent here because 'id' is a required property for the method below and Event class doesnt have that method.
                await Client.ReplaceDocumentAsync(eventDoc.SelfLink, evt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static async Task<bool> DeleteEvent(string eventId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", eventId));
            Document eventDoc = Client.CreateDocumentQuery(EventsCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null)
                return false;

            try
            {
                await Client.DeleteDocumentAsync(eventDoc.SelfLink);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static IEnumerable<DBEvent> GetEventsForUser(string userName)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.OwnerId = \"{0}\") ", userName));
            return Client.CreateDocumentQuery<DBEvent>(EventsCollection.DocumentsLink, querySpec).AsEnumerable();
        }

        internal static IEnumerable<DBEvent> GetEventsForCampaign(string campaignId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.CampaignId = \"{0}\") ", campaignId));
            return Client.CreateDocumentQuery<DBEvent>(EventsCollection.DocumentsLink, querySpec).AsEnumerable();
        }

        internal static IEnumerable<DBComment> GetCommentsForUser(string userName)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.OwnerId = \"{0}\") ", userName));
            return Client.CreateDocumentQuery<DBComment>(CommentsCollection.DocumentsLink, querySpec).AsEnumerable();
        }

        internal static IEnumerable<DBComment> GetCommentsForCampaign(string campaignId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.CampaignId = \"{0}\") ", campaignId));
            return Client.CreateDocumentQuery<DBComment>(CommentsCollection.DocumentsLink, querySpec).AsEnumerable();
        }

        internal static async Task<bool> DeleteUser(string strUserName)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", strUserName));
            Document doc = Client.CreateDocumentQuery(UsersCollection.DocumentsLink, querySpec).AsEnumerable().FirstOrDefault();

            if (doc == null)
                return false;
            try
            {
                var response = await Client.DeleteDocumentAsync(doc.SelfLink);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}