using System;
using System.Collections.Generic;
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

namespace Acty.Service.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class CampaignsController : DocumentController<DBCampaign>
    {
        NotificationController _notificationController = new NotificationController();
        private static string _campaignsStorageUrl;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _campaignsStorageUrl = ConfigurationManager.AppSettings["CampaignsStorageUrl"];
            DomainManager = new DocumentEntityDomainManager<DBCampaign>("campaigns", Request, Services);
        }

        internal void TestInitialize(HttpControllerContext controllerContext)
        {
            Initialize(controllerContext);
        }

        [Route("api/createCampaign")]
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
                return CreateCampaignInDB(story).Result;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private async Task<bool> CreateCampaignInDB(Campaign campaign)
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
                StoryMediaResourceBlob = uniqueFileName,
                MinAge = campaign.MinAge,
                ContentName = campaign.ContentName,
                ContentType = campaign.ContentType
            };

            var createdStory = await InsertAsync(dbStory);
            if (createdStory == null)
                return false;

            return true;
        }

        [Route("api/getCampaignsForUser")]
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

        [Route("api/getTopFeedsForUser")]
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


        #region Commented to have sample of notificationController usage
        /*
        // PATCH tables/Challenge/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Challenge> PatchChallenge(string id, Delta<Challenge> patch)
        {
            var challenge = patch.GetEntity();
            _authController.EnsureHasPermission(new User[] { challenge.ChallengeeAthlete, challenge.ChallengerAthlete }, Request);

            return UpdateAsync(id, patch);
        }

        // POST tables/Challenge
        public async Task<IHttpActionResult> PostChallenge(ChallengeDto item)
        {
            var challenger = _context.Users.SingleOrDefault(a => a.Id == item.ChallengerAthleteId);
            var challengee = _context.Users.SingleOrDefault(a => a.Id == item.ChallengeeAthleteId);

            _authController.EnsureHasPermission(new User[] { challenger, challengee }, Request);

            if(challenger == null || challengee == null)
                throw "The opponent in this challenge no longer belongs to this league".ToException(Request);

            var challengerMembership = _context.Memberships.SingleOrDefault(m => m.AbandonDate == null && m.AthleteId == challenger.Id && m.LeagueId == item.LeagueId);
            var challengeeMembership = _context.Memberships.SingleOrDefault(m => m.AbandonDate == null && m.AthleteId == challengee.Id && m.LeagueId == item.LeagueId);

            if(challengerMembership == null || challengeeMembership == null)
                throw "The opponent in this challenge no longer belongs to this league".ToException(Request);

            //Check to see if there are any ongoing challenges between either athlete
            var challengeeOngoing = _context.Challenges.Where(c => (c.ChallengeeAthleteId == item.ChallengeeAthleteId || c.ChallengerAthleteId == item.ChallengeeAthleteId)
                && c.LeagueId == item.LeagueId && c.DateCompleted == null);

            if(challengeeOngoing.Count() > 0)
                throw "{0} already has an existing challenge underway.".Fmt(challengee.DisplayName).ToException(Request);

            var challengerOngoing = _context.Challenges.Where(c => (c.ChallengerAthleteId == item.ChallengerAthleteId || c.ChallengeeAthleteId == item.ChallengerAthleteId)
                && c.LeagueId == item.LeagueId && c.DateCompleted == null);

            if(challengerOngoing.Count() > 0)
                throw "You already have an existing challenge underway.".ToException(Request);

            //Check to see if there is already a challenge between the two athletes for this league
            var history = _context.Challenges.Where(c => ((c.ChallengeeAthleteId == item.ChallengeeAthleteId && c.ChallengerAthleteId == item.ChallengerAthleteId)
                || (c.ChallengeeAthleteId == item.ChallengerAthleteId && c.ChallengerAthleteId == item.ChallengeeAthleteId))
                && c.LeagueId == item.LeagueId).OrderByDescending(c => c.DateCompleted);

            var league = _context.Leagues.SingleOrDefault(l => l.Id == item.LeagueId);
            var lastChallenge = history.FirstOrDefault();
            
            if(lastChallenge != null && lastChallenge.DateCompleted != null
                && lastChallenge.ChallengerAthleteId == item.ChallengerAthleteId //is it the same athlete challenging again
                && lastChallenge.GetChallengerWinningGames().Count() < lastChallenge.GetChallengeeWinningGames().Count() //did the challenger lose the previous match
                && DateTime.UtcNow.Subtract(lastChallenge.DateCompleted.Value.UtcDateTime).TotalHours < league.MinHoursBetweenChallenge) //has enough time passed
            {
                throw "You must wait at least {0} hours before challenging again".Fmt(league.MinHoursBetweenChallenge).ToException(Request);
            }

            Challenge current = await InsertAsync(item.ToChallenge());
            var result = CreatedAtRoute("Tables", new { id = current.Id }, current.ToChallengeDto());

            var message = "{0}: You have been challenged to a duel by {1}!".Fmt(league.Name, challenger.DisplayName);
            var payload = new NotificationPayload
            {
                Action = PushActions.ChallengePosted,
                Payload = { { "challengeId", current.Id }, { "leagueId", current.LeagueId } }
            };

            //Not awaiting so the user's result is not delayed
            _notificationController.NotifyByTag(message, current.ChallengeeAthleteId, payload);
            return result;
        }

        [HttpGet]
        [Route("api/revokeChallenge")]
        public async Task RevokeChallenge(string id)
        {
            var challenge = Lookup(id).Queryable.FirstOrDefault();

            if(challenge == null)
                return;

            _authController.EnsureHasPermission(new[] { challenge.ChallengeeAthlete, challenge.ChallengerAthlete }, Request);
            var message = "Your challenge with {0} has been revoked.".Fmt(challenge.ChallengerAthlete.DisplayName);
            var payload = new NotificationPayload
            {
                Action = PushActions.ChallengeRevoked,
                Payload = { { "challengeId", id }, { "leagueId", challenge.LeagueId } }
            };

            await _notificationController.NotifyByTag(message, challenge.ChallengeeAthleteId, payload);
            await DeleteAsync(id);
        }

        [HttpGet]
        [Route("api/declineChallenge")]
        public async Task DeclineChallenge(string id)
        {
            var challenge = Lookup(id).Queryable.FirstOrDefault();

            if(challenge == null)
                return;

            _authController.EnsureHasPermission(new [] { challenge.ChallengeeAthlete, challenge.ChallengerAthlete }, Request);

            var message = "Your challenge with {0} has been declined.".Fmt(challenge.ChallengeeAthlete.DisplayName);
            var payload = new NotificationPayload
            {
                Action = PushActions.ChallengeDeclined,
                Payload = { { "challengeId", id }, { "leagueId", challenge.LeagueId } }
            };

            await _notificationController.NotifyByTag(message, challenge.ChallengerAthleteId, payload);
            await DeleteAsync(id);
        }

        [Route("api/acceptChallenge")]
        async public Task<ChallengeDto> AcceptChallenge(string id)
        {
            var challenge = _context.Challenges.SingleOrDefault(c => c.Id == id);

            if(challenge == null)
                throw "This challenge no longer exists".ToException(Request);

            _authController.EnsureHasPermission(challenge.ChallengeeAthlete, Request);
            challenge.DateAccepted = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var league = _context.Leagues.SingleOrDefault(l => l.Id == challenge.LeagueId);
            var challengee = _context.Users.SingleOrDefault(a => a.Id == challenge.ChallengeeAthleteId);
            var message = "{0}: Your challenge with {1} has been accepted!".Fmt(league.Name, challengee.DisplayName);
            var payload = new NotificationPayload
            {
                Action = PushActions.ChallengeAccepted,
                Payload = { { "challengeId", id }, { "leagueId", challenge.LeagueId } }
            };

            await _notificationController.NotifyByTag(message, challenge.ChallengerAthleteId, payload);
            return challenge.ToChallengeDto();
        }

        [HttpGet]
        [Route("api/nagAthlete")]
        public async Task NudgeAthlete(string challengeId)
        {
            var challenge = Lookup(challengeId).Queryable.FirstOrDefault();

            if(challenge == null)
                throw "This challenge no longer exists".ToException(Request);

            _authController.EnsureHasPermission(challenge.ChallengerAthlete, Request);

            if (challenge.ChallengerAthlete == null)
                throw "The challenger no longer exists".ToException(Request);

            var message = "{0} would be much obliged if you'd accept their challenge.".Fmt(challenge.ChallengerAthlete.DisplayName);
            var payload = new NotificationPayload
            {
                Action = PushActions.ChallengePosted,
                Payload = { { "challengeId", challengeId },    { "leagueId", challenge.LeagueId } }
            };

            await _notificationController.NotifyByTag(message, challenge.ChallengeeAthleteId, payload);
        }

        [Route("api/postMatchResults")]
        async public Task<ChallengeDto> PostMatchResults(List<GameResultDto> results)
        {
            if(results.Count < 1)
                throw "No game scores were submitted.".ToException(Request);

            var challengeId = results.First().ChallengeId;
            var challenge = _context.Challenges.SingleOrDefault(c => c.Id == challengeId);

            if(challenge == null)
                throw "This challenge no longer exists".ToException(Request);

            if(challenge.DateCompleted != null)
                throw "Scores for this challenge have already been submitted.".ToException(Request);

            var league = _context.Leagues.SingleOrDefault(l => l.Id == challenge.LeagueId);

            if(league == null)
                throw "This league no longer exists".ToException(Request);
            
            if(challenge.ChallengerAthlete == null || challenge.ChallengeeAthlete == null)
                throw "The opponent in this challenge no longer belongs to this league".ToException(Request);

            var challengerMembership = _context.Memberships.SingleOrDefault(m => m.AthleteId == challenge.ChallengerAthlete.Id && m.AbandonDate== null && m.LeagueId == challenge.LeagueId);
            var challengeeMembership = _context.Memberships.SingleOrDefault(m => m.AthleteId == challenge.ChallengeeAthlete.Id && m.AbandonDate == null && m.LeagueId == challenge.LeagueId);

            if(challengerMembership == null || challengeeMembership == null)
                throw "The opponent in this challenge no longer belongs to this league".ToException(Request);

            _authController.EnsureHasPermission(new[] { challenge.ChallengerAthlete, challenge.ChallengeeAthlete }, Request);

            var tempChallenge = new Challenge();
            tempChallenge.League = league;
            tempChallenge.MatchResult = results.Select(g => g.ToGameResult()).ToList();

            var errorMessage = tempChallenge.ValidateMatchResults();

            if(errorMessage != null)
                throw errorMessage.ToException(Request);

            tempChallenge = null;
            challenge.DateCompleted = DateTime.UtcNow;
            var dto = challenge.ToChallengeDto();
            dto.MatchResult = new List<GameResultDto>();

            foreach(var result in results)
            {
                result.Id = Guid.NewGuid().ToString();
                _context.GameResults.Add(result.ToGameResult());
                dto.MatchResult.Add(result);
            }

            try
            {
                _context.SaveChanges();
                var challengerWins = challenge.GetChallengerWinningGames();
                var challengeeWins = challenge.GetChallengeeWinningGames();
                var winningRank = challengeeMembership.CurrentRank;

                User winner = challenge.ChallengeeAthlete;
                User loser = challenge.ChallengerAthlete;

                if(challengerWins.Length > challengeeWins.Length)
                {
                    winner = challenge.ChallengerAthlete;
                    loser = challenge.ChallengeeAthlete;

                    var oldRank = challengerMembership.CurrentRank;
                    challengerMembership.CurrentRank = challengeeMembership.CurrentRank;
                    challengeeMembership.CurrentRank = oldRank;
                    challengerMembership.LastRankChange = DateTime.UtcNow;
                    challengeeMembership.LastRankChange = DateTime.UtcNow;
                    winningRank = challengerMembership.CurrentRank;

                    _context.SaveChanges();
                }

                var maintain = winner.Id == challenge.ChallengerAthlete.Id ? "bequeath" : "retain";
                var newRank = winningRank + 1;
                var message = "{0} victors over {1} to {2} the righteous rank of {3} place in {4}".Fmt(winner.DisplayName, loser.DisplayName, maintain, newRank.ToOrdinal(), league.Name);
                var payload = new NotificationPayload
                {
                    Action = PushActions.ChallengeCompleted,
                    Payload = { { "challengeId", challengeId },
                        { "leagueId", league.Id },
                        {"winningAthleteId", winner.Id},
                        {"losingAthleteId", loser.Id} }
                };

                _notificationController.NotifyByTag(message, challenge.LeagueId, payload);
            }
            catch(DbEntityValidationException e)
            {
                #region Error Print

                foreach(var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach(var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

                #endregion
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return dto;
        }
        */
        #endregion
    }
}