using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentDBDataService;
using System.Collections.Generic;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class CommentsTests
    {
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
                Email = "hemanth@gmail.com",
                Password = "xzssddddd",
                ZipCode = "98007",
                Preferences = new UserPreferences() { InterestedLocales = new List<string>() { "US/98007", "US/98052", "IN/577005" } }
            };

            _theService = new ActiDataService();
        }

        [TestMethod]
        public void TestAddDeleteComment()
        {
            IEnumerable<DBCampaign> campaigns = _theService.GetCampaignsForUser(_user1.Email);
            var enumerator = campaigns.GetEnumerator();
            DBCampaign campaign = null;
            do
            {
                if (enumerator.Current != null)
                {
                    campaign = enumerator.Current;
                    break;
                }
            } while (enumerator.MoveNext());

            Assert.IsNotNull(campaign);

            Comment comment = new Comment() 
            { CampaignId = campaign.id,
              CreatedDate = DateTime.Now.ToString(),
              Description = "I strongly support this thesis",
              OwnerId = "notHemanth@gmail.com" };

            Assert.IsTrue(_theService.AddComment(Helper.ConvertObjectToStream(comment)));

            var comments = _theService.GetCommentsForUser("notHemanth@gmail.com");
            var etor = comments.GetEnumerator();
            DBComment dbComment = null;
            do
            {
                if (etor.Current != null)
                {
                    dbComment = etor.Current;
                    break;
                }
            } while (etor.MoveNext());
            Assert.IsNotNull(dbComment);

            Comment replyComment = new Comment()
            {
                CampaignId = campaign.id,
                CreatedDate = DateTime.Now.ToString(),
                Description = "I strongly accept your support",
                OwnerId = "hemanth@gmail.com",
                ReplyCommentId = dbComment.id
            };
            Assert.IsTrue(_theService.AddComment(Helper.ConvertObjectToStream(replyComment)));

            // Delete All created comments.
            comments = _theService.GetCommentsForCampaign(campaign.id);
            etor = comments.GetEnumerator();
            do
            {
                if (etor.Current != null)
                    Assert.IsTrue(_theService.DeleteComment(etor.Current.id));
            } while (etor.MoveNext());
        }
    }
}
