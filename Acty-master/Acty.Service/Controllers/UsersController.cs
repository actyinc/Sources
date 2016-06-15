using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Acty.Service.HttpMultipartParser;
using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace Acty.Service.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class UsersController : DocumentController<DBUser>
    {
        AuthenticationController _authController = new AuthenticationController();
        ////private static string _usersStorageUrl = ConfigurationManager.AppSettings["UsersStorageUrl"];

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new DocumentEntityDomainManager<DBUser>("users", Request, Services);
        }

        internal void TestInitialize(HttpControllerContext controllerContext)
        {
            Initialize(controllerContext);
        }

        public IQueryable<DBUser> GetAllUsers()
        {
            return Query();
        }

        [Route("api/getUser")]
        public DBUser GetUser(string userName)
        {
            try
            {
                return DocumentDBConnector.GetUser(userName);
            }
            catch (Exception ex)
            {
                if (ex.Message == null)
                    return null;
            }
            return null;
        }

        ////public Task<DBUser> ReplaceUser(string id, DBUser user)
        ////{
        ////    return ReplaceAsync(id, user);
        ////}

        ////public async Task<IHttpActionResult> PostUser(DBUser item)
        ////{
        ////    var doc = await InsertAsync(item);

        ////    return CreatedAtRoute("DefaultApis", new { id = doc.Id }, doc);
        ////}

        [Route("api/deleteUser")]
        public async Task<bool> DeleteUser(string userName)
        {
            if (!_authController.HasPermission(userName))
                return false;

            return await DeleteUserTest666(userName);
        }

        internal async Task<bool> DeleteUserTest666(string userName)
        {
            return await DocumentDBConnector.DeleteUser(userName);
        }

        [Route("api/createUserProfile")]
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

        ////////private async Task<bool> AddUser(User user)
        ////////{
        ////////    var userPic = user.ProfilePicture;
        ////////    string uniqueFileName = "";
        ////////    if (userPic != null)
        ////////    {
        ////////        uniqueFileName = userPic.FileName + "_" + Guid.NewGuid();
        ////////        await StorageConnector.UploadUserData(uniqueFileName, userPic.ContentType, userPic.Data);
        ////////    }

        ////////    DBUser dbUser = new DBUser()
        ////////    {
        ////////        Country = user.Country,
        ////////        DisplayName = user.DisplayName,
        ////////        DOB = user.DOB,
        ////////        Email = user.Email,
        ////////        Password = user.Password,
        ////////        ZipCode = user.ZipCode,
        ////////        UserProfilePictureBlob = uniqueFileName,
        ////////        CreatedDate = user.CreatedDate,
        ////////        Preferences = user.Preferences
        ////////    };

        ////////    var createdUser = await InsertAsync(dbUser);

        ////////    if (createdUser == null)
        ////////        return false;

        ////////    return true;
        ////////}

        [Route("api/isUserNameAvailable")]
        public bool IsUserNameAvailable(string strUserName)
        {
            if (string.IsNullOrWhiteSpace(strUserName) || string.IsNullOrEmpty(strUserName))
                return false;

            return DocumentDBConnector.IsUserNameAvailable(strUserName);
        }

        [Route("api/updateUserPicture")]
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

        ////////private async Task<bool> UpdateUserProfilePicture(UserPicture userPic)
        ////////{
        ////////    SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", userPic.UserId));
        ////////    var userDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

        ////////    if (userDoc == null)
        ////////        return false;

        ////////    try
        ////////    {
        ////////        // Delete the previous profile picture if its already present.
        ////////        var blobKey = userDoc.GetPropertyValue<string>(Constants.UserProfilePictureBlobKey);
        ////////        if (!string.IsNullOrEmpty(blobKey))
        ////////            await StorageConnector.DeleteUserProfilePicture(blobKey);

        ////////        string uniqueFileName = userPic.FileName + "_" + Guid.NewGuid();
        ////////        await StorageConnector.UploadUserData(uniqueFileName, userPic.ContentType, userPic.Data);
        ////////        userDoc.SetPropertyValue(Constants.UserProfilePictureBlobKey, uniqueFileName);
        ////////        await ReplaceAsync(userDoc.SelfLink, userDoc);
        ////////    }
        ////////    catch (Exception)
        ////////    {
        ////////        return false;
        ////////    }

        ////////    return true;
        ////////}

        [Route("api/addUserPreference")]
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

        [Route("api/removeUserPreference")]
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

        // If isAdd is false, its considered as remove.
        ////////private async Task<bool> UpdateUserPreference(User user, bool isAdd)
        ////////{
        ////////    SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.Email = \"{0}\") ", user.Email));
        ////////    var doc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

        ////////    if (doc == null)
        ////////        return false;

        ////////    try
        ////////    {
        ////////        if (isAdd)
        ////////            doc.Preferences.InterestedLocales.AddRange(user.Preferences.InterestedLocales);
        ////////        else
        ////////            foreach (var item in user.Preferences.InterestedLocales)
        ////////                doc.Preferences.InterestedLocales.Remove(item);

        ////////        doc.SetPropertyValue(Constants.UserPreferencesKey, doc.Preferences);
        ////////        await ReplaceAsync(doc.SelfLink, doc);
        ////////    }
        ////////    catch (Exception)
        ////////    {
        ////////        return false;
        ////////    }

        ////////    return true;
        ////////}
    }
}