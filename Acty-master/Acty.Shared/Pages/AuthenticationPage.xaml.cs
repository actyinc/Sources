using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Acty.Shared
{
    public partial class AuthenticationPage : AuthenticationPageXaml
    {
        public AuthenticationPage()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            InitializeComponent();
            Title = "Authenticating";
        }

        async public Task<bool> AttemptToAuthenticateAthlete(bool force = false)
        {
            try
            {
                await ViewModel.AuthenticateCompletely();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<App>(App.Current, ex.Message);
            }


            if(App.CurrentAthlete != null)
            {
                MessagingCenter.Send<App>(App.Current, Messages.AuthenticationComplete);
            }

            return App.CurrentAthlete != null;
        }
    }

    public partial class AuthenticationPageXaml : BaseContentPage<AuthenticationViewModel>
    {
    }
}