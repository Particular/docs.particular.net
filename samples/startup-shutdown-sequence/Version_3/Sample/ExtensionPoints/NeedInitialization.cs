using NServiceBus.Config;

public class NeedInitialization :
    INeedInitialization
{
    public void Init()
    {
        Logger.WriteLine("Inside INeedInitialization.Init");
    }
}