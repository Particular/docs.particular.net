namespace NServiceBus
{
    public interface ISessionProvider
    {
        IMessageSession GetSession(string endpointName);
    }
}