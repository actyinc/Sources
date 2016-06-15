using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Acty.Shared;
using Xamarin;
using Xamarin.Forms;
using System.Text;

[assembly: Dependency(typeof(Acty.Android.Authentication))]

namespace Acty.Android
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> DisplayWebView()
        {
            try
            {
                var muser = await AzureService.Instance.Client.LoginAsync(Forms.Context, MobileServiceAuthenticationProvider.Google);
                //muser.MobileServiceAuthenticationToken = "ZXlKMGVYQWlPaUpLVjFRaUxDSmhiR2NpT2lKSVV6STFOaUo5.ZXlKcGMzTWlPaUoxY200NmJXbGpjbTl6YjJaME9uZHBibVJ2ZDNNdFlYcDFjbVU2ZW5WdGJ5SXNJbUYxWkNJNkluVnlianB0YVdOeWIzTnZablE2ZDJsdVpHOTNjeTFoZW5WeVpUcDZkVzF2SWl3aWJtSm1Jam94TkRZek16Y3hNREEzTENKbGVIQWlPakUwTmpVNU5qTXdNRGNzSW5WeWJqcHRhV055YjNOdlpuUTZZM0psWkdWdWRHbGhiSE1pT2lKN2ZTSXNJblZwWkNJNklrZHZiMmRzWlRveE1USTBNVE0yTVRneU56UTRNRFV5T1RreU1qUWlMQ0oyWlhJaU9pSXlJbjA=.YnM5LW9kdlozM1poVktyOXRGUXVQaUtUSjZ6eWJKRGhPU0pUbHVBc2p1QQ==";
                return muser;
            }
            catch(Exception e)
            {
                InsightsManager.Report(e);
            }

            return null;
        }

        public void ClearCookies()
        {
            global::Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
        }
    }
}