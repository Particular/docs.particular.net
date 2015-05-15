using NServiceBus;
using NServiceBus.Unicast.Queuing;

public class WantQueueCreated :
    IWantQueueCreated
{
    public Address Address
    {
        get { return null; }
    }

    public bool IsDisabled
    {
        get
        {
            Logger.WriteLine("Inside IWantQueueCreated");
            return true;
        }
    }

}