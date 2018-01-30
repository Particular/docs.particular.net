#region rotating-session-key-provider
public class RotatingSessionKeyProvider : ISessionKeyProvider
{
    readonly string[] sessionKeys = {
        "Particular",
        "Messaging",
        "NServiceBus",
        "Sagas"
    };

    int currentKeyIndex;

    public void NextKey()
    {
        currentKeyIndex = (currentKeyIndex + 1) % sessionKeys.Length;
    }

    public string SessionKey => sessionKeys[currentKeyIndex];
}
#endregion