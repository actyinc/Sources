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
    public class CommentsController : DocumentController<DBComment>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DomainManager = new DocumentEntityDomainManager<DBComment>("comments", Request, Services);
        }

        internal void TestInitialize(HttpControllerContext controllerContext)
        {
            Initialize(controllerContext);
        }

        /// <summary>
        /// Currently this just adds the comment. but in reality, the moment you add the comment correcponding 
        /// Campaign's comment count and the replyComment count (is this is a reply to a comment) should be incremented.
        /// this can be accomplished by stored procedures which the documentDB supports. Need to investigate on that.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("api/addComment")]
        public bool AddComment(Stream data)
        {
            // convert Stream Data to StreamReader
            StreamReader reader = new StreamReader(data);
            // Read StreamReader data as string
            string jsonString = reader.ReadToEnd();
            Comment comment = JsonConvert.DeserializeObject<Comment>(jsonString);
            var result = InsertAsync(comment.ToDBComment()).Result;

            // Currently this just adds the comment. but in reality, the moment you add the comment correcponding 
            // Campaign's comment count and the replyComment count (is this is a reply to a comment) should be incremented.
            // this can be accomplished by stored procedures which the documentDB supports. Need to investigate on that.
            if (result == null)
                return false;

            return true;
        }

        /// <summary>
        /// Similarly delete Comment Should update all the comment count in events and delete all the replies to the comment.
        /// </summary>
        [Route("api/deleteComment")]
        public bool DeleteComment(string commentId)
        {
            SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.id = \"{0}\") ", commentId));
            var commentDoc = DomainManager.Query(querySpec).AsEnumerable().FirstOrDefault();

            // Similarly delete Comment Should update all the comment count in events and delete all the replies to the comment.
            if (commentDoc == null)
                return false;

            try
            {
                DeleteAsync(commentDoc.SelfLink).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Route("api/getCommentsFromUser")]
        public IEnumerable<DBComment> GetCommentsFromUser(string userName)
        {
            try
            {
                SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.ownerId = \"{0}\") ", userName));
                return DomainManager.Query(querySpec).AsEnumerable();
            }
            catch (Exception)
            { }
            return null;
        }


        [Route("api/getCommentsForCampaign")]
        public IEnumerable<DBComment> GetCommentsForCampaign(string campaignId)
        {
            try
            {
                SqlQuerySpec querySpec = new SqlQuerySpec(string.Format("SELECT * FROM root WHERE (root.campaignId = \"{0}\") ", campaignId));
                return DomainManager.Query(querySpec).AsEnumerable();
            }
            catch (Exception)
            { }
            return null;
        }
    }
}