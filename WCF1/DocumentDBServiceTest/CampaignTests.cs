using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentDBDataService;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class CampaignTests
    {
        private const string _campaignViedo1 = "\\Data\\Video\\Campaign1.wmv";
        private const string _campaignImage1 = "\\Data\\Image\\Preethi.jpg";
        private User _user1;
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
                Email = "hemhedge@gmail.com",
                Password = "xzssddddd",
                ZipCode = "98007",
                Preferences = new UserPreferences() { InterestedLocales = new List<string>() { "US/98007", "US/98052", "IN/577005" } }
            };

            _theService = new ActiDataService();
        }

        [TestMethod]
        public void TestCreateVideoCampaign()
        {
            var dbUser = _theService.GetUser(_user1.Email);
            var story = new Campaign()
            {
                Country = "US",
                ZipCode = "98007",
                CreatedDate = DateTime.Now.ToString(),
                LastUpdatedDate = DateTime.Now.ToString(),
                OwnerId = dbUser.Email,
                Heading = "Why Capitalism and Democracy have failed us.",
                Category = CampaignCategory.Political.ToString(),
                IsLocal = true,
                KeyWords = new string[3] {"Political","Capitalism","Democracy"},
                Message = "Here I explain why they fail us and what we can do to achieve that ideal society.",
                Status = CampaignStatus.Active.ToString(),
                MinAge = 4,
            };

            TestCampaignMedia cm = new TestCampaignMedia() { UserId = dbUser.id };
            string path = Directory.GetCurrentDirectory() + _campaignViedo1;
            var fi = new FileInfo(path);
            long length = fi.Length;
            cm.ContentLength = length;
            cm.FileName = fi.Name;
            cm.ContentType = "video/x-ms-wmv";
            cm.Data = Helper.ConvertFileToByteArray(path, length);

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("OwnerId", story.OwnerId);
            postParameters.Add("CreatedDate", story.CreatedDate);
            postParameters.Add("LastUpdatedDate", story.LastUpdatedDate);
            postParameters.Add("Heading", story.Heading);
            postParameters.Add("Category", story.Category);
            postParameters.Add("Message", story.Message);
            postParameters.Add("IsLocal", story.IsLocal);
            postParameters.Add("ZipCode", story.ZipCode);
            postParameters.Add("Country", story.Country);
            postParameters.Add("Keywords", string.Join(",", story.KeyWords));
            postParameters.Add("MinAge", story.MinAge);
            postParameters.Add("FileName", cm.FileName);
            postParameters.Add("ContentType", cm.ContentType);
            postParameters.Add("ContentLength", cm.ContentLength);
            postParameters.Add("Status", story.Status);
            postParameters.Add("File", new FormUpload.FileParameter(cm.Data, cm.FileName, cm.ContentType));

            Assert.IsTrue(_theService.CreateCampaign(new MemoryStream(FormUpload.MultipartFormDataPost(postParameters))));
        }

        [TestMethod]
        public void TestCreateImageCampaign()
        {
            var dbUser = _theService.GetUser(_user1.Email);
            var story = new Campaign()
            {
                Country = "US",
                ZipCode = "98007",
                CreatedDate = DateTime.Now.ToString(),
                LastUpdatedDate = DateTime.Now.ToString(),
                OwnerId = dbUser.Email,
                Heading = "Why is it soo hard to convince people truths about global warming",
                Category = CampaignCategory.Political.ToString(),
                IsLocal = true,
                KeyWords = new string[3] { "Environment", "Global Warming", "IceAge" },
                Message = "The main reason is our own stupidity.",
                Status = CampaignStatus.Active.ToString(),
                MinAge = 4,
            };

            TestCampaignMedia cm = new TestCampaignMedia() { UserId = dbUser.id };
            string path = Directory.GetCurrentDirectory() + _campaignImage1;
            var fi = new FileInfo(path);
            long length = fi.Length;
            cm.ContentLength = length;
            cm.FileName = fi.Name;
            cm.ContentType = "image/jpeg";
            cm.Data = Helper.ConvertFileToByteArray(path, length);

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("OwnerId", story.OwnerId);
            postParameters.Add("CreatedDate", story.CreatedDate);
            postParameters.Add("LastUpdatedDate", story.LastUpdatedDate);
            postParameters.Add("Heading", story.Heading);
            postParameters.Add("Category", story.Category);
            postParameters.Add("Message", story.Message);
            postParameters.Add("IsLocal", story.IsLocal);
            postParameters.Add("ZipCode", story.ZipCode);
            postParameters.Add("Country", story.Country);
            postParameters.Add("Keywords", string.Join(",", story.KeyWords));
            postParameters.Add("MinAge", story.MinAge);
            postParameters.Add("FileName", cm.FileName);
            postParameters.Add("ContentType", cm.ContentType);
            postParameters.Add("ContentLength", cm.ContentLength);
            postParameters.Add("Status", story.Status);
            postParameters.Add("File", new FormUpload.FileParameter(cm.Data, cm.FileName, cm.ContentType));

            Assert.IsTrue(_theService.CreateCampaign(new MemoryStream(FormUpload.MultipartFormDataPost(postParameters))));
        }

        [TestMethod]
        public void TestGetCampaignsForUser()
        {
            IEnumerable<DBCampaign> campaigns = _theService.GetCampaignsForUser(_user1.Email);
            var enumerator = campaigns.GetEnumerator();
            int count = 0;
            do
            {
                if (enumerator.Current != null)
                    count++;
            } while (enumerator.MoveNext());
                
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void TestTopFeedForUser()
        {
            IEnumerable<DBCampaign> feeds = _theService.GetTopFeedsForUser(_user1.Email);
            var enumerator = feeds.GetEnumerator();
            int count = 0;
            do
            {
                if (enumerator.Current != null)
                    count++;
            } while (enumerator.MoveNext());
            Assert.IsTrue(count > 0);
        }
    }
}
