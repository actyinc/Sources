using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace Acty.Service.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class EventsController : DocumentController<DBEvent>
    {
        AuthenticationController _authController = new AuthenticationController();
        NotificationController _notificationController = new NotificationController();

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new DocumentEntityDomainManager<DBEvent>("eventsdb", Request, Services);
        }

        internal void TestInitialize(HttpControllerContext controllerContext)
        {
            Initialize(controllerContext);
        }

        [Route("api/addEvent")]
        public bool AddEvent(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            Event evt = JsonConvert.DeserializeObject<Event>(jsonString);
            var createdEvt = InsertAsync(evt.ToDBEvent()).Result;

            if (createdEvt == null)
                return false;

            return true;
        }

        [Route("api/updateEvent")]
        public bool UpdateEvent(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            DBEvent evt = JsonConvert.DeserializeObject<DBEvent>(jsonString);

            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", evt.id));
            var eventDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null || !_authController.HasPermission(eventDoc.OwnerId))
                return false;

            try
            {
                ReplaceAsync(eventDoc.SelfLink, evt).Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool UpdateEventTest666(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            DBEvent evt = JsonConvert.DeserializeObject<DBEvent>(jsonString);

            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", evt.id));
            var eventDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null)
                return false;

            try
            {
                ReplaceAsync(eventDoc.SelfLink, evt).Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/deleteEvent")]
        public bool DeleteEvent(string eventId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", eventId));
            var eventDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null || !_authController.HasPermission(eventDoc.OwnerId))
                return false;

            try
            {
                DeleteAsync(eventDoc.SelfLink).Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool DeleteEventTest666(string eventId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", eventId));
            var eventDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

            if (eventDoc == null)
                return false;

            try
            {
                DeleteAsync(eventDoc.SelfLink).Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("api/getEventsForUser")]
        public List<DBEvent> GetEventsForUser(string userName)
        {
            try
            {
                SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.ownerId = \"{0}\") ", userName));
                return DomainManager.Query(querySpec).AsEnumerable().ToList();
            }
            catch (Exception)
            { }
            return null;
        }

        [Route("api/getEventsForCampaign")]
        public List<DBEvent> GetEventsForCampaign(string campaignId)
        {
            try
            {
                SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.campaignId = \"{0}\") ", campaignId));
                return DomainManager.Query(querySpec).AsEnumerable().ToList();
            }
            catch (Exception)
            { }
            return null;
        }

        #region Commented to have sample of notificationController usage
        /*
        IQueryable<LeagueDto> ConvertLeagueToDto(IQueryable<League> queryable)
        {
            return queryable.Select(l => new LeagueDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Sport = l.Sport,
                IsEnabled = l.IsEnabled,
                DateCreated = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                RulesUrl = l.RulesUrl,
                CreatedByAthleteId = l.CreatedByAthlete.Id,
                ImageUrl = l.ImageUrl,
                Season = l.Season,
                MaxChallengeRange = l.MaxChallengeRange,
                MinHoursBetweenChallenge = l.MinHoursBetweenChallenge,
                MatchGameCount = l.MatchGameCount,
                HasStarted = l.HasStarted,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                IsAcceptingMembers = l.IsAcceptingMembers,
                Memberships = l.Memberships.Where(m => m.AbandonDate == null).OrderBy(m => m.CurrentRank).Select(m => new MembershipDto
                {
                    Id = m.Id,
                    UpdatedAt = m.UpdatedAt,
                    AthleteId = m.Athlete.Id,
                    LeagueId = m.League.Id,
                    IsAdmin = m.IsAdmin,
                    CurrentRank = m.CurrentRank,
                    LastRankChange = m.LastRankChange,
                    DateCreated = m.CreatedAt,
                }).ToList(),
                OngoingChallenges = l.Challenges.ToList().Where(c => c.DateCompleted == null).OrderBy(c => c.ProposedTime).Select(c => new ChallengeDto
                {
                    Id = c.Id,
                    ChallengerAthleteId = c.ChallengerAthleteId,
                    ChallengeeAthleteId = c.ChallengeeAthleteId,
                    LeagueId = c.LeagueId,
                    BattleForRank = c.BattleForRank,
                    DateCreated = c.CreatedAt,
                    ProposedTime = c.ProposedTime,
                    UpdatedAt = c.UpdatedAt,
                    DateAccepted = c.DateAccepted,
                }).ToList(),
            });
        }

        // GET tables/League
        public IQueryable<LeagueDto> GetAllLeagues()
        {
            return ConvertLeagueToDto(Query());
        }

        // GET tables/League/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<LeagueDto> GetLeague(string id)
        {
            return SingleResult<LeagueDto>.Create(ConvertLeagueToDto(Lookup(id).Queryable));
        }

        [Route("api/startLeague")]
        public DateTime StartLeague(string id)
        {
            _authController.EnsureAdmin(Request);
            var league = _context.Leagues.SingleOrDefault(l => l.Id == id);
            league.HasStarted = true;
            league.StartDate = DateTime.Now.ToUniversalTime();

            var memberships = _context.Memberships.Where(m => m.LeagueId == id && m.AbandonDate == null).ToList();

            if(memberships.Count < 2)
            {
                //TODO Enable this validation
                //return Conflict("Must have at least 2 members before starting a league.");
            }

            memberships.Shuffle();

            //Randomize the athlete rankage when the league kicks off
            var i = 0;
            foreach(var m in memberships)
            {
                m.CurrentRank = i;
                i++;
            }
            _context.SaveChanges();

            var message = "The {0} league has officially started!".Fmt(league.Name);
            var payload = new NotificationPayload
            {
                Action = PushActions.LeagueStarted,
                Payload = { { "leagueId", id } }
            };

            _notificationController.NotifyByTag(message, league.Id, payload);
            return league.StartDate.Value.UtcDateTime;
        }

        // PATCH tables/League/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<League> PatchLeague(string id, Delta<League> patch)
        {
            _authController.EnsureAdmin(Request);
            var league = _context.Leagues.SingleOrDefault(l => l.Id == id);

            var updated = patch.GetEntity();
            if(!league.IsAcceptingMembers && updated.IsAcceptingMembers)
            {
                //NotifyAboutNewLeagueOpenEnrollment(updated);
            }

            return UpdateAsync(id, patch);
        }

        void NotifyAboutNewLeagueOpenEnrollment(League league)
        {
            var date = league.StartDate.Value.DateTime.ToOrdinal();
            var message = "Open enrollment for the {0} league has started. The league will begin on {1}".Fmt(league.Name, date);
            var payload = new NotificationPayload
            {
                Action = PushActions.LeagueStarted,
                Payload = { { "leagueId", league.Id } }
            };
            _notificationController.NotifyByTag(message, "All", payload);
        }

        // POST tables/League
        public async Task<IHttpActionResult> PostLeague(LeagueDto item)
        {
            _authController.EnsureAdmin(Request);
            var exists = _context.Leagues.Any(l => l.Name.Equals(item.Name, System.StringComparison.InvariantCultureIgnoreCase));

            if(exists)
                return BadRequest("The name of that league is already in use.");

            League league = await InsertAsync(item.ToLeague());

            if(league.IsAcceptingMembers)
            {
                NotifyAboutNewLeagueOpenEnrollment(league);
            }

            return CreatedAtRoute("Tables", new { id = league.Id }, league);
        }

        // DELETE tables/League/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteLeague(string id)
        {
            _authController.EnsureAdmin(Request);
            var league = _context.Leagues.SingleOrDefault(l => l.Id == id);
            var message = "The {0} league has been removed.".Fmt(league.Name);
            var payload = new NotificationPayload
            {
                Action = PushActions.LeagueEnded,
                Payload = { { "leagueId", id } }
            };
            _notificationController.NotifyByTag(message, league.Id, payload);

            return DeleteAsync(id);
        }
        */
        #endregion
    }
}
 