using NServiceBus.Unicast;

public class WantToRunWhenTheBusStarts :
    IWantToRunWhenTheBusStarts
{
    public void Run()
    {
        Logger.WriteLine("Inside IWantToRunWhenTheBusStarts.Run");
    }

}
