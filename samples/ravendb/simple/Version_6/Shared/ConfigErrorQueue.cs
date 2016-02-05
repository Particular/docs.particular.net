using NServiceBus;

class ConfigErrorQueue : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.SendFailedMessagesTo("error");
    }
}