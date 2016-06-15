using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Acty.Shared
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> DisplayWebView();

        void ClearCookies();
    }
}