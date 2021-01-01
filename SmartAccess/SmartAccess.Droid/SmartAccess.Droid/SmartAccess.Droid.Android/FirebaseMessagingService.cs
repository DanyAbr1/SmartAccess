using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmartAccess
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        AndroidNotificationManager androidNotification = new AndroidNotificationManager();
        public override void OnMessageReceived(RemoteMessage message)
        {

            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            androidNotification.CrearNotificacionLocal(message.GetNotification().Title, message.GetNotification().Body);

           FlashlightNotication();
        }

        private void FlashlightNotication()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                for (int i = 0; i < 6; i++)
                {
                    // Turn On                   
                     Flashlight.TurnOnAsync();
                     Vibration.Vibrate(15);
                    // Turn Off
                    Thread.Sleep(5000);
                    Task.Delay(TimeSpan.FromMilliseconds(1500));
                    Flashlight.TurnOffAsync();

                }
            });
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);

            Preferences.Set("TokenFirebase", token);
            sedRegisterToken(token);
        }



        public void sedRegisterToken(string token)
        {
            //Tu código para registrar el token a tu servidor y base de datos
        }
    }
}
