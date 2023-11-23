namespace api.Controllers;

/// <summary>contains a list of notification which can be filled up, and send back to the front end all at once.</summary>
public abstract class Notification
{
    public System.Collections.Generic.ICollection<string> _notifications = new System.Collections.Generic.List<string>();

    public void AddErrorMessage(string notifications)
    {
        _notifications.Add(notifications);
    }

    public bool HasErrors() => _notifications.Count > 0;
}

// Since this is a highly unknown (and in my opinion underestimated pattern), I'll leave its origin and explanation here: https://martinfowler.com/articles/replaceThrowWithNotification.html
