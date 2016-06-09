using NServiceBus;

public class NeedInitialization :
    INeedInitialization
{
    void INeedInitialization.Init()
    {
        Logger.WriteLine("Inside INeedInitialization.Init");
    }
}