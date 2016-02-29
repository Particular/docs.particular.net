using NServiceBus;

public class NeedInitialization :
    INeedInitialization
{
    public void Customize(BusConfiguration busConfiguration)
    {
        Logger.WriteLine("Inside INeedInitialization.Customize");
    }
}