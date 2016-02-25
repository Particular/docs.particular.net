using NServiceBus;

class ConfigErrorQueue : INeedInitialization
{
    public void Customize(EndpointConfiguration configuration)
    {
        configuration.SendFailedMessagesTo("error");
    }
}