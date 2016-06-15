using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using Acty;
using Acty.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class CommentsTests
    {
        private User _user1;
        private CommentsController _commentsController;
        private CampaignsController _campaignsController;

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

            HttpControllerContext context = new HttpControllerContext();
            _campaignsController = new CampaignsController();
            _campaignsController.TestInitialize(context);
            _commentsController = new CommentsController();
            _commentsController.TestInitialize(context);
        }

        [TestMethod]
        public void TestAddDeleteComment()
        {
            IEnumerable<DBCampaign> campaigns = _campaignsController.GetCampaignsForUser(_user1.Email);
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

            Assert.IsTrue(_commentsController.AddComment(Helper.ConvertObjectToStream(comment)));

            var comments = _commentsController.GetCommentsFromUser("notHemanth@gmail.com");
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
            Assert.IsTrue(_commentsController.AddComment(Helper.ConvertObjectToStream(replyComment)));

            // Delete All created comments.
            comments = _commentsController.GetCommentsForCampaign(campaign.id);
            etor = comments.GetEnumerator();
            do
            {
                if (etor.Current != null)
                    Assert.IsTrue(_commentsController.DeleteComment(etor.Current.id));
            } while (etor.MoveNext());
        }
    }
}
