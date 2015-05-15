using NServiceBus;

public class NeedInitialization :
    INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        Logger.WriteLine("Inside INeedInitialization.Customize");
    }
}