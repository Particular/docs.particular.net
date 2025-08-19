#region session-key-provider-interface
public interface ISessionKeyProvider
{
    void NextKey();
    string SessionKey { get; }
}
#endregion