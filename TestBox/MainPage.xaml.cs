namespace TestBox;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
        {
            string msg = m.Value;
        });

        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", "");
        }

        if (Preferences.ContainsKey("NavigationId"))
        {
            string id = Preferences.Get("NavigationId", "");
            if (id.Equals("1"))
            {
                AppShell.Current.GoToAsync(nameof(DetailsPage));
            }
        }
    }

    private string _deviceToken;

    private void OnCounterClicked(object sender, EventArgs e)
    {
        var pushNotificationRequest = new PushNotificationRequest
        {
            notification = new NotificationMessageBody
            {
                title = "Notification Title",
                body = "Notification Body"
            },

            registration_ids = new List<string> { _deviceToken}
        };

        string url = "https://fcm.googleapis.com/fcm/send";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "ZKS5GB56A2");
        }
    }
}
