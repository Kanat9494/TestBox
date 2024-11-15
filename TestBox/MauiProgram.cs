using Microsoft.Extensions.Logging;

namespace TestBox
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) => // для выполнения определенных действия после того как получает уведомление
            {
                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) => // используется когда устройство обновляется - для получения актуального токена
            {
                System.Diagnostics.Debug.WriteLine($"Token: {p.Token}");
                // send to our server
            };
            return builder.Build();
        }
    }
}
