using System;
using Acty.Shared;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Widget;

[assembly: Dependency(typeof(Acty.Android.ToastNotifier))]

namespace Acty.Android
{
    public class ToastNotifier : IToastNotifier
    {
        public Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            Toast.MakeText(Forms.Context, description, ToastLength.Short).Show();
            return taskCompletionSource.Task;
        }

        public void HideAll()
        {
        }
    }
}