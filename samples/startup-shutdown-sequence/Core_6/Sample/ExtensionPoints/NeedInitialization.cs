using NServiceBus;

public class NeedInitialization :
    INeedInitialization
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        Logger.WriteLine("Inside INeedInitialization.Customize");
    }
}