using Foundation;
using Acty.Shared;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Acty.iOS.PushNotifications))]

namespace Acty.iOS
{
    public class PushNotifications : IPushNotifications
    {
        public void RegisterForPushNotifications()
        {
            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert
                           | UIUserNotificationType.Badge
                           | UIUserNotificationType.Sound, new NSSet());


            if (settings != null)
            {
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
        }

        public bool IsRegistered
        {
            get
            {
                return UIApplication.SharedApplication.IsRegisteredForRemoteNotifications;
            }
        }
    }
}

