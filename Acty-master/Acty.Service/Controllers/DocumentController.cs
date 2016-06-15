using Microsoft.Azure.Documents;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace Acty.Service.Controllers
{
    public abstract class DocumentController<TDocument> : ApiController where TDocument : Resource
    {
        public ApiServices Services { get; set; }

        private DocumentEntityDomainManager<TDocument> _domainManager;
        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.WindowsAzure.Mobile.Service.Tables.IDomainManager`1" /> to be used for accessing the backend store.
        /// </summary>
        protected DocumentEntityDomainManager<TDocument> DomainManager
        {
            get
            {
                if (_domainManager == null)
                {
                    throw new InvalidOperationException("Domain manager not set");
                }
                return _domainManager;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _domainManager = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.WindowsAzure.Mobile.Service.TableController`1" /> class.
        /// </summary>
        protected DocumentController()
        {
        }


        protected virtual IQueryable<TDocument> Query()
        {
            IQueryable<TDocument> result;
            try
            {
                result = DomainManager.Query();
            }
            catch (HttpResponseException exception)
            {
                Services.Log.Error(exception, base.Request, LogCategories.Controllers);
                throw;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex, base.Request, LogCategories.Controllers);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            return result;
        }

        protected virtual SingleResult<TDocument> Lookup(string id)
        {

            try
            {
                return DomainManager.Lookup(id);
            }
            catch (HttpResponseException exception)
            {
                Services.Log.Error(exception, base.Request, LogCategories.TableControllers);
                throw;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex, base.Request, LogCategories.TableControllers);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        protected async virtual Task<Document> InsertAsync(TDocument item)
        {
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


            try
            {
                return await DomainManager.InsertAsync(item);

            }
            catch (HttpResponseException exception)
            {
                Services.Log.Error(exception, base.Request, LogCategories.Controllers);
                throw;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex, base.Request, LogCategories.Controllers);

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        protected async virtual Task<TDocument> ReplaceAsync(string selflink, TDocument item)
        {
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            TDocument result;

            try
            {
                var flag = await DomainManager.ReplaceAsync(selflink, item);

                if (!flag)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                result = item;
            }
            catch (HttpResponseException exception)
            {
                Services.Log.Error(exception, base.Request, LogCategories.TableControllers);
                throw;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex, base.Request, LogCategories.TableControllers);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            return result;
        }

        protected virtual async Task DeleteAsync(string selfLink)
        {
            bool flag = false;
            try
            {
                flag = await DomainManager.DeleteAsync(selfLink);

            }
            catch (HttpResponseException exception)
            {
                Services.Log.Error(exception, base.Request, LogCategories.TableControllers);
                throw;
            }
            catch (Exception ex)
            {
                Services.Log.Error(ex, base.Request, LogCategories.TableControllers);

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            if (!flag)
            {
                Services.Log.Warn("Resource not found", base.Request);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


        }

    }
}