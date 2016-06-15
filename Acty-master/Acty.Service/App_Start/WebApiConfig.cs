using Microsoft.WindowsAzure.Mobile.Service;
//using Acty.Service.Models;
using System.Web.Http;
//using Acty.Service.Migrations;

namespace Acty.Service
{
    public static class WebApiConfig
    {
        public static bool IsDemoMode
        {
            get;
            set;
        }

        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();
            //options.LoginProviders.Remove(typeof(GoogleLoginAuthenticationProvider));
            //options.LoginProviders.Add(typeof(GoogleLoginAuthenticationProvider));

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            bool isDemoMode;
            var boolString = System.Configuration.ConfigurationManager.AppSettings["IsDemoMode"];
            if(bool.TryParse(boolString, out isDemoMode))
                IsDemoMode = isDemoMode;

            //config.SetIsHosted(true);
            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            //var migrator = new DbMigrator(new Configuration());
            //migrator.Update();
        }
    }
}

