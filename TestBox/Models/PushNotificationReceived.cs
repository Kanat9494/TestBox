namespace TestBox.Models;

public class PushNotificationReceived : ValueChangedMessage<string>
{
    public PushNotificationReceived(string message) : base(message) { }
}
