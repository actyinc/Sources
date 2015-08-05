using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;
using DocumentDBDataService.DocumentDB;
using DocumentDBDataService.HttpMultipartParser;
using System.ServiceModel.Web;

namespace DocumentDBDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActiDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ActiDataService.svc or ActiDataService.svc.cs at the Solution Explorer and start debugging.
    public class ActiDataService : IActiDataService
    {
        public bool CreateUserProfile(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            var userObj = JsonConvert.DeserializeObject<User>(jsonString);
            var userAdded = DocumentDBConnector.AddUser(userObj).Result;
            return userAdded;
        }

        public bool IsUserNameAvailable(string strUserName)
        {
            return DocumentDBConnector.IsUserNameAvailable(strUserName);
        }

        public bool DeleteUser(string strUserName)
        {
            return DocumentDBConnector.DeleteUser(strUserName).Result;
        }

        public bool UpdateUserPicture(Stream data)
        {
            try
            {
                var parser = new MultipartFormDataParser(data, Encoding.UTF8);
                var file = parser.Files.First();
                string fileName = file.FileName;
                Stream fileData = file.Data;

                var userPic = new UserPicture()
                {
                    UserId = parser.Parameters[Constants.UserIdKey].Data,
                    ContentLength = long.Parse(parser.Parameters[Constants.ContentLengthKey].Data),
                    Data = fileData,
                    ContentType = parser.Parameters[Constants.ContentTypeKey].Data,
                    FileName = parser.Parameters[Constants.FileNameKey].Data
                };
                return DocumentDBConnector.UpdateUserProfilePicture(userPic).Result;
            }
            catch (Exception) { }
            return false;
        }

        public bool AddUserPreference(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            User userObj = JsonConvert.DeserializeObject<User>(jsonString);
            var userAdded = DocumentDBConnector.UpdateUserPreference(userObj, true).Result;
            return userAdded;
        }

        public bool RemoveUserPreference(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            User userObj = JsonConvert.DeserializeObject<User>(jsonString);
            var userAdded = DocumentDBConnector.UpdateUserPreference(userObj, false).Result;
            return userAdded;
        }


        public bool CreateCampaign(Stream data)
        {
            try
            {
                //MultiPartParser.ParseFiles(data, WebOperationContext.Current.IncomingRequest.ContentType, ProcessStory).Wait();
                var parser = new MultipartFormDataParser(data, Encoding.UTF8);
                var file = parser.Files.First();
                string fileName = file.FileName;
                Stream fileData = file.Data;

                var story = new Campaign()
                {
                    OwnerId = parser.Parameters[Constants.OwnerIdKey].Data,
                    Category = parser.Parameters[Constants.CategoryKey].Data,
                    Message = parser.Parameters[Constants.MessageKey].Data,
                    KeyWords = parser.Parameters[Constants.KeyWordsKey].Data.Split(new char[] { ',' }),
                    Country = parser.Parameters[Constants.CountryKey].Data,
                    CreatedDate = parser.Parameters[Constants.CreatedDateKey].Data,
                    IsLocal = parser.Parameters[Constants.IsLocalKey].Data == "true",
                    Heading = parser.Parameters[Constants.HeadingKey].Data,
                    ZipCode = parser.Parameters[Constants.ZipCodeKey].Data,
                    Status = parser.Parameters[Constants.StatusKey].Data,
                    MinAge = int.Parse(parser.Parameters[Constants.MinAgeKey].Data),
                    ContentName = parser.Parameters[Constants.FileNameKey].Data,
                    ContentType = parser.Parameters[Constants.ContentTypeKey].Data,
                    CampaignVisualResource = new CampaignMedia()
                    {
                        ContentLength = long.Parse(parser.Parameters[Constants.ContentLengthKey].Data),
                        ContentType = parser.Parameters[Constants.ContentTypeKey].Data,
                        FileName = parser.Parameters[Constants.FileNameKey].Data,
                        UserId = parser.Parameters[Constants.OwnerIdKey].Data,
                        Data = fileData
                    }
                };
                return DocumentDBConnector.CreateCampaignInDB(story).Result;
            }
            catch (Exception)
            { }
            return false;
        }

        public DBUser GetUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetUser(userName);
            }
            catch (Exception)
            { }
            return null;
        }

        public IEnumerable<DBCampaign> GetCampaignsForUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetCampaignsForUser(userName);
            }
            catch (Exception)
            { }
            return null;
        }

        public IEnumerable<DBCampaign> GetTopFeedsForUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetTopFeedsForUser(userName);
            }
            catch (Exception)
            { }
            return null;
        }

        public bool AddComment(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            Comment comment = JsonConvert.DeserializeObject<Comment>(jsonString);
            return DocumentDBConnector.AddComment(comment).Result;
        }

        public bool DeleteComment(string commentId)
        {
            return DocumentDBConnector.DeleteComment(commentId).Result;
        }

        public bool AddEvent(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            Event evt = JsonConvert.DeserializeObject<Event>(jsonString);
            return DocumentDBConnector.AddEvent(evt).Result;
        }

        public bool UpdateEvent(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            DBEvent evt = JsonConvert.DeserializeObject<DBEvent>(jsonString);
            return DocumentDBConnector.UpdateEvent(evt).Result;
        }

        public bool DeleteEvent(string eventId)
        {
            return DocumentDBConnector.DeleteEvent(eventId).Result;
        }

        public IEnumerable<DBEvent> GetEventsForUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetEventsForUser(userName);
            }
            catch (Exception)
            { }
            return null;
        }

        public IEnumerable<DBEvent> GetEventsForCampaign(string campaignId)
        {
            try
            {
                return DocumentDBConnector.GetEventsForCampaign(campaignId);
            }
            catch (Exception)
            { }
            return null;
        }

        public IEnumerable<DBComment> GetCommentsForUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetCommentsForUser(userName);
            }
            catch (Exception)
            { }
            return null;
        }

        public IEnumerable<DBComment> GetCommentsForCampaign(string campaignId)
        {
            try
            {
                return DocumentDBConnector.GetCommentsForCampaign(campaignId);
            }
            catch (Exception)
            { }
            return null;
        }
    }
}
