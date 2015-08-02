using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using DocumentDBDataService;
using System.Text;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class UserTests
    {
        private const string _hemanthProfilePic = "\\Data\\Image\\Hemanth.jpg";
        private const string _preethiProfilePic = "\\Data\\Image\\Preethi.jpg";
        private User _user1;
        private User _user2;
        private ActiDataService _theService;

        [TestInitialize]
        public void Initialize()
        {
            _user1 = new User()
            {
                Country = "US",
                CreatedDate = DateTime.Now.ToString(),
                DisplayName = "Hemanth",
                DOB = DateTime.Parse("1/1/1984").ToString(),
                Email = "hemanth@gmail.com",
                Password = "xzssddddd",
                ZipCode = "98007",
                Preferences = new UserPreferences() { InterestedLocales = new List<string>() { "US/98007", "US/98052", "IN/577005"} }
            };

            _user2 = new User()
            {
                Country = "US",
                CreatedDate = DateTime.Now.ToString(),
                DisplayName = "Preethi",
                DOB = DateTime.Parse("1/1/1985").ToString(),
                Email = "preethi@gmail.com",
                Password = "xzssddddd",
                ZipCode = "98052",
                Preferences = new UserPreferences() { InterestedLocales = new List<string>() { "US/98007", "US/98052", "IN/577005" } }
            };

            _theService = new ActiDataService();
        }

        [TestMethod]
        public void TestCreateUser()
        {
            Helper.DeleteUserIfExists(_user1, _theService);
            Assert.IsTrue(_theService.CreateUserProfile(Helper.ConvertObjectToStream(_user1)));
            Helper.CheckUserExists(_user1, _theService);

            Helper.DeleteUserIfExists(_user2, _theService);
            Assert.IsTrue(_theService.CreateUserProfile(Helper.ConvertObjectToStream(_user2)));
            Helper.CheckUserExists(_user2, _theService);
        }

        [TestMethod]
        public void TestUpdateUserProfilePicture()
        {
            Helper.CheckUserExists(_user1, _theService);
            TestUserPicture up = new TestUserPicture() { UserId = _user1.Email };
            string path = Directory.GetCurrentDirectory() + _hemanthProfilePic;
            var fi = new FileInfo(path);
            long length = fi.Length;
            up.ContentLength = length;
            up.FileName = fi.Name;
            up.ContentType = "image/jpeg";
            up.Data = Helper.ConvertFileToByteArray(path, length);

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("UserId", up.UserId);
            postParameters.Add("FileName", up.FileName);
            postParameters.Add("ContentType", up.ContentType);
            postParameters.Add("ContentLength", up.ContentLength);
            postParameters.Add("File", new FormUpload.FileParameter(up.Data, up.FileName, up.ContentType));

            Assert.IsTrue(_theService.UpdateUserPicture(new MemoryStream(FormUpload.MultipartFormDataPost(postParameters))));
        }

        [TestMethod]
        public void TestIsUserNameAvailable()
        {
            Helper.CheckUserExists(_user1, _theService);
            Assert.IsFalse(_theService.IsUserNameAvailable(_user1.Email));
            Assert.IsTrue(_theService.IsUserNameAvailable("fshkgfhlufhgrklsjrdfghjksfdlgh"));
        }

        [TestMethod]
        public void TestAddRemoveUserPreference()
        {
            var dbUser = _theService.GetUser(_user1.Email);
            int originalCount = _user1.Preferences.InterestedLocales.Count;
            Assert.AreEqual(originalCount, dbUser.Preferences.InterestedLocales.Count);
            var preference = new UserPreferences() { InterestedLocales = new List<string>()};
            preference.AddInterestedLocale("NZ","200985");
            _user1.Preferences = preference;
            _theService.AddUserPreference(Helper.ConvertObjectToStream(_user1));
            dbUser = _theService.GetUser(_user1.Email);
            Assert.AreEqual(originalCount + 1, dbUser.Preferences.InterestedLocales.Count);
            Assert.IsTrue(dbUser.Preferences.InterestedLocales.Contains("NZ/200985"));

            // Remove
            _theService.RemoveUserPreference(Helper.ConvertObjectToStream(_user1));
            dbUser = _theService.GetUser(_user1.Email);
            Assert.AreEqual(originalCount, dbUser.Preferences.InterestedLocales.Count);
            Assert.IsFalse(dbUser.Preferences.InterestedLocales.Contains("NZ/200985"));
        }
    }
}
