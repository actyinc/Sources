using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using Acty;
using Acty.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentDBServiceTest
{
    [TestClass]
    public class EventsTests
    {
        private User _user1;
        private EventsController _eventsController;
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
            _eventsController = new EventsController();
            _eventsController.TestInitialize(context);
        }

        [TestMethod]
        public void TestAddDeleteEvent()
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
            string nowTime = DateTime.Now.ToString();
            Event evt = new Event()
            {
                CampaignId = campaign.id,
                CreatedDate = nowTime,
                LastUpdatedDate = nowTime,
                Description = "Lets meet discuss the plan of action",
                OwnerId = campaign.OwnerId,
                ZipCode = "98007",
                Time = "7/29/2015 3:30 PM",
                Country = "US",
                KeyWords = new string[3] { "Do something","Politics","Environmental" },
                Location = "Sammamish"
            };

            Assert.IsTrue(_eventsController.AddEvent(Helper.ConvertObjectToStream(evt)));

            var evts = _eventsController.GetEventsForUser(campaign.OwnerId);
            var etor = evts.GetEnumerator();
            int count = 0;
            DBEvent dbevt = null;
            do
            {
                if (etor.Current != null)
                {
                    dbevt = etor.Current;
                    count++;
                }
            } while (etor.MoveNext());
            Assert.IsNotNull(dbevt);
            Assert.AreEqual(1, count);

            evts = _eventsController.GetEventsForCampaign(campaign.id);
            etor = evts.GetEnumerator();
            DBEvent dbevt_c = null;
            count = 0;
            do
            {
                if (etor.Current != null)
                {
                    dbevt_c = etor.Current;
                    count++;
                }
            } while (etor.MoveNext());
            Assert.IsNotNull(dbevt_c);
            Assert.AreEqual(1, count);
            Assert.AreEqual(dbevt.id, dbevt_c.id);
            Assert.AreEqual(dbevt.id, dbevt_c.id);
            Assert.AreEqual(dbevt.Description, dbevt_c.Description);
            Assert.AreEqual(dbevt.OwnerId, dbevt_c.OwnerId);
            Assert.AreEqual(dbevt.CreatedDate, dbevt_c.CreatedDate);
            Assert.AreEqual(dbevt.LastUpdatedDate, dbevt_c.LastUpdatedDate);

            // Delete All created events.
            Assert.IsTrue(_eventsController.DeleteEventTest666(dbevt_c.id));
        }

        [TestMethod]
        public void TestUpdateEvent()
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

            // Cleanup all the old events
            var evts = _eventsController.GetEventsForCampaign(campaign.id);
            var etor = evts.GetEnumerator();
            do
            {
                if (etor.Current != null)
                    Assert.IsTrue(_eventsController.DeleteEvent(etor.Current.id));
            } while (etor.MoveNext());

            // Now start the test.
            Assert.IsNotNull(campaign);
            string nowTime = DateTime.Now.ToString();
            Event evt = new Event()
            {
                CampaignId = campaign.id,
                CreatedDate = nowTime,
                LastUpdatedDate = nowTime,
                Description = "Lets meet discuss the plan of action",
                OwnerId = "hemanth@gmail.com",
                ZipCode = "98007",
                Time = "7/29/2015 3:30 PM",
                Country = "US",
                KeyWords = new string[3] { "Do something", "Politics", "Environmental" },
                Location = "Sammamish"
            };

            Assert.IsTrue(_eventsController.AddEvent(Helper.ConvertObjectToStream(evt)));

            evts = _eventsController.GetEventsForUser("hemanth@gmail.com");
            etor = evts.GetEnumerator();
            int count = 0;
            DBEvent dbevt = null;
            do
            {
                if (etor.Current != null)
                {
                    dbevt = etor.Current;
                    count++;
                }
            } while (etor.MoveNext());
            Assert.IsNotNull(dbevt);
            Assert.AreEqual(1, count);

            string newNowTime = DateTime.Parse("7/29/2015 3:30:20 PM").ToString();
            dbevt.LastUpdatedDate = newNowTime;
            dbevt.Location = "Issaquah";
            Assert.IsTrue(_eventsController.UpdateEventTest666(Helper.ConvertObjectToStream(dbevt)));

            evts = _eventsController.GetEventsForCampaign(campaign.id);
            etor = evts.GetEnumerator();
            DBEvent dbevt_c = null;
            count = 0;
            do
            {
                if (etor.Current != null)
                {
                    dbevt_c = etor.Current;
                    count++;
                }
            } while (etor.MoveNext());
            Assert.IsNotNull(dbevt_c);
            Assert.AreEqual(1, count);
            Assert.AreEqual(newNowTime, dbevt_c.LastUpdatedDate);
            Assert.AreEqual("Issaquah", dbevt_c.Location);

            // Delete All created events.
            Assert.IsTrue(_eventsController.DeleteEventTest666(dbevt_c.id));
        }
    }
}
