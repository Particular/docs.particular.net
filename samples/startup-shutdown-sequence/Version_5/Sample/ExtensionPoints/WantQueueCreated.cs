using NServiceBus;
using NServiceBus.Unicast.Queuing;

public class WantQueueCreated :
    IWantQueueCreated
{
    public bool ShouldCreateQueue()
    {
        Logger.WriteLine("Inside IWantQueueCreated");
        return false;
    }

    public Address Address
    {
        get { return null; }
    }
}