using NServiceBus;


public class SendOnly
{
    public void Simple()
    {
        #region SendOnly 5

        var configuration = new BusConfiguration();
        
        var bus = Bus.CreateSendOnly(configuration);

        #endregion
    }

}