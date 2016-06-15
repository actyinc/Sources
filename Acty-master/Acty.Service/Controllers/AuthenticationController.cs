using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace Acty.Service.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class AuthenticationController : DocumentController<DBUser>
    {
        [Route("api/getUserIdentity")]
        public async Task<GoogleCredentials> GetUserIdentity()
        {
            ServiceUser serviceUser = User as ServiceUser;
            if(serviceUser != null)
            {
                var identity = await serviceUser.GetIdentitiesAsync();
                var credentials = identity.OfType<GoogleCredentials>().FirstOrDefault();
                return credentials;
            }

            return null;
        }

        string _userId;
        public string UserId
        {
            get
            {
                if(_userId == null) { }
                {
                    var identity = GetUserIdentity().Result;
                    if(identity != null)
                        _userId = identity.UserId;
                }

                return _userId;
            }
        }

        public bool IsCurrentUser(User athlete)
        {
            if(athlete == null)
                return false;

            var identity = GetUserIdentity().Result;
            if(identity == null)
                return false;

            return athlete.Email == identity.UserId;
        }
        public void EnsureHasPermission(User athlete, HttpRequestMessage request)
        {
            EnsureHasPermission(new User[] { athlete }, request);
        }

        public void EnsureHasPermission(User[] athletes, HttpRequestMessage request)
        {
            foreach(var a in athletes)
            {
                if(a != null && a.Email == UserId)
                    return;
            }

            throw "Invalid permission".ToException(request);
        }

        public bool HasPermission(string userId)
        {
            if (userId == UserId)
                return true;

            return false;
        }
    }
}