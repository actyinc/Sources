using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.WindowsAzure.Mobile.Service;

namespace Acty.Service
{
    public class DocumentEntityDomainManager<TDocument> where TDocument : Resource
    {
        public HttpRequestMessage Request { get; set; }
        public ApiServices Services { get; set; }

        private DocumentCollection _collection;

        public DocumentEntityDomainManager(string collectionName, HttpRequestMessage request, ApiServices services)
        {
            Request = request;
            Services = services;
            _collection = DocumentDBConnector.GetCollection(collectionName);
        }

        public async Task<bool> DeleteAsync(string selfLink)
        {
            try
            {
                var response = await Client.DeleteDocumentAsync(selfLink);

                return response.Resource == null;


            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Document> InsertAsync(TDocument data)
        {
            try
            {
                return await Client.CreateDocumentAsync(Collection.SelfLink, data);


            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public SingleResult<TDocument> Lookup(string id)
        {
            try
            {
                return SingleResult.Create<TDocument>(
                    Client.CreateDocumentQuery<TDocument>(Collection.DocumentsLink)
                    .Where(d => d.Id == id)
                    .Select<TDocument, TDocument>(d => d)
                    );


            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public IQueryable<TDocument> Query()
        {
            try
            {
                return Client.CreateDocumentQuery<TDocument>(Collection.DocumentsLink);
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public IQueryable<TDocument> Query(SqlQuerySpec querySpec)
        {
            try
            {
                return Client.CreateDocumentQuery<TDocument>(Collection.DocumentsLink, querySpec);
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> ReplaceAsync(string selfLink, TDocument item)
        {

            if (item == null || string.IsNullOrEmpty(selfLink))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }

            try
            {
                var response = await Client.ReplaceDocumentAsync(selfLink, item);

                return response.Resource != null;

            }
            catch (Exception ex)
            {
                Services.Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        #region DocumentDBClient

        internal DocumentClient Client
        {
            get
            {
                return DocumentDBConnector.Client;
            }
        }

        internal DocumentCollection Collection
        {
            get
            {
                return _collection;
            }
        }
        #endregion

    }
}