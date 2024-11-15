using Android.App;
using Android.Content.PM;
using Android.Gms.Tasks;
using Android.OS;
using Firebase.Messaging;
using Firebase;
using TestBox.Platforms.Android;
using Android.Util;

namespace TestBox;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance { get; private set; }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Instance = this;

        NotificationPermissionHelper.RequestNotificationPermissionAsync().ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Console.WriteLine("Ошибка при запросе разрешения на уведомления: " + t.Exception?.Message);
            }
            else if (t.IsCompletedSuccessfully)
            {
                Console.WriteLine("Запрос разрешения на уведомления завершен");
            }
        });
        //----
        // Initialize Firebase
        FirebaseApp.InitializeApp(this);
        //var firebaseApp = FirebaseApp.Instance;
        //if (firebaseApp == null)
        //{
        //    Console.WriteLine("Ошибка: Firebase не инициализирован.");
        //    return;
        //}
        // канал уведомления для Android 8.0 и выше
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel("default_channel", "Default Channel", NotificationImportance.Default)
            {
                Description = "Default Notification Channel"
            };
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        // Obtain the Firebase Cloud Messaging token
        FirebaseMessaging.Instance.GetToken().AddOnCompleteListener(new OnCompleteListener());

        //CreateNotificationChannel();
    }

    internal static readonly string CHANNEL_ID = "TestChannel";
    internal static readonly int NOTIFICATION_ID = 101;

    private class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                string token = task.Result.ToString();
                Log.Debug("FCM Token", token);
                FCMTokenSingleton.Instance.FCMToken = token;
            }
            else
            {
                Log.Warn("FCM Token", "Fetching FCM registration token failed", task.Exception);
            }
        }
    }
}


