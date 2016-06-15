using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http.Controllers;
using Acty;
using Acty.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class UserTests
    {
        private const string _hemanthProfilePic = "\\Data\\Image\\Hemanth.jpg";
        private const string _preethiProfilePic = "\\Data\\Image\\Preethi.jpg";
        private User _user1;
        private User _user2;
        private UsersController _usersController;

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

            HttpControllerContext context = new HttpControllerContext();
            _usersController = new UsersController();
            _usersController.TestInitialize(context);
        }

        [TestMethod]
        public void TestCreateUser()
        {
            Helper.DeleteUserIfExists(_user1, _usersController);
            Assert.IsTrue(_usersController.CreateUserProfile(Helper.ConvertObjectToStream(_user1)));
            Helper.CheckUserExists(_user1, _usersController);

            Helper.DeleteUserIfExists(_user2, _usersController);
            Assert.IsTrue(_usersController.CreateUserProfile(Helper.ConvertObjectToStream(_user2)));
            Helper.CheckUserExists(_user2, _usersController);
        }

        [TestMethod]
        public void TestUpdateUserProfilePicture()
        {
            Helper.CheckUserExists(_user1, _usersController);
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

            Assert.IsTrue(_usersController.UpdateUserPicture(new MemoryStream(FormUpload.MultipartFormDataPost(postParameters))));
        }

        [TestMethod]
        public void TestIsUserNameAvailable()
        {
            Helper.CheckUserExists(_user1, _usersController);
            Assert.IsFalse(_usersController.IsUserNameAvailable(_user1.Email));
            Assert.IsTrue(_usersController.IsUserNameAvailable("fshkgfhlufhgrklsjrdfghjksfdlgh"));
        }

        [TestMethod]
        public void TestAddRemoveUserPreference()
        {
            var dbUser = _usersController.GetUser(_user1.Email);
            int originalCount = dbUser.Preferences.InterestedLocales.Count;

            var preference = new UserPreferences() { InterestedLocales = new List<string>()};
            preference.AddInterestedLocale("NZ","200985");
            dbUser.Preferences = preference;
            _usersController.AddUserPreference(Helper.ConvertObjectToStream(dbUser));

            dbUser = _usersController.GetUser(dbUser.Email);
            Assert.AreEqual(originalCount + 1, dbUser.Preferences.InterestedLocales.Count);
            Assert.IsTrue(dbUser.Preferences.InterestedLocales.Contains("NZ/200985"));

            // Remove
            dbUser.Preferences = preference;
            _usersController.RemoveUserPreference(Helper.ConvertObjectToStream(dbUser));
            dbUser = _usersController.GetUser(dbUser.Email);
            Assert.AreEqual(originalCount, dbUser.Preferences.InterestedLocales.Count);
            Assert.IsFalse(dbUser.Preferences.InterestedLocales.Contains("NZ/200985"));
        }
    }
}
