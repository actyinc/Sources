using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DocumentDBDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IActiDataService" in both code and config file together.
    [ServiceContract]
    public interface IActiDataService
    {
        [OperationContract(Name = "CreateUserProfile")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "CreateUserProfile")]
        bool CreateUserProfile(Stream data);

        [OperationContract(Name = "IsUserNameAvailable")]
        [WebGet(UriTemplate = "IsUserNameAvailable/userName/{name}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool IsUserNameAvailable(string name);

        [OperationContract(Name = "DeleteUser")]
        [WebGet(UriTemplate = "DeleteUser/userName/{name}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool DeleteUser(string name);

        [OperationContract(Name = "UpdateUserPicture")]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateUserPicture")]
        bool UpdateUserPicture(Stream data);

        [OperationContract(Name = "RemoveUserPreference")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "RemoveUserPreference")]
        bool RemoveUserPreference(Stream data);

        [OperationContract(Name = "AddUserPreference")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddUserPreference")]
        bool AddUserPreference(Stream data);

        [OperationContract(Name = "CreateCampaign")]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateCampaign")]
        bool CreateCampaign(Stream data);

        [OperationContract(Name = "GetUser")]
        [WebGet(UriTemplate = "GetUser/userName/{userName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        DBUser GetUser(string userName);

        [OperationContract(Name = "GetCampaignsForUser")]
        [WebGet(UriTemplate = "GetCampaignsForUser/userName/{userName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBCampaign> GetCampaignsForUser(string userName);

        // Currently this returns only the campaigns, but long term it should return 'Feed' object which could contain,
        // campaigns, comments, likes etc..
        [OperationContract(Name = "GetTopFeedsForUser")]
        [WebGet(UriTemplate = "GetTopFeedsForUser/userName/{userName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBCampaign> GetTopFeedsForUser(string userName);

        [OperationContract(Name = "AddComment")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddComment")]
        bool AddComment(Stream data);

        [OperationContract(Name = "DeleteComment")]
        [WebGet(UriTemplate = "DeleteComment/commentId/{commentId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool DeleteComment(string commentId);

        [OperationContract(Name = "AddEvent")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "AddEvent")]
        bool AddEvent(Stream data);

        [OperationContract(Name = "UpdateEvent")]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateEvent")]
        bool UpdateEvent(Stream data);

        [OperationContract(Name = "DeleteEvent")]
        [WebGet(UriTemplate = "DeleteEvent/eventId/{eventId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        bool DeleteEvent(string eventId);

        [OperationContract(Name = "GetEventsForUser")]
        [WebGet(UriTemplate = "GetEventsForUser/userName/{userName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBEvent> GetEventsForUser(string userName);

        [OperationContract(Name = "GetEventsForCampaign")]
        [WebGet(UriTemplate = "GetEventsForCampaign/campaignId/{campaignId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBEvent> GetEventsForCampaign(string campaignId);

        [OperationContract(Name = "GetCommentsForUser")]
        [WebGet(UriTemplate = "GetCommentsForUser/userName/{userName}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBComment> GetCommentsForUser(string userName);

        [OperationContract(Name = "GetCommentsForCampaign")]
        [WebGet(UriTemplate = "GetCommentsForCampaign/campaignId/{campaignId}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        IEnumerable<DBComment> GetCommentsForCampaign(string campaignId);
    }
}
