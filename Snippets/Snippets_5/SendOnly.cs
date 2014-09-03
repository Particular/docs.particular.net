using NServiceBus;


public class SendOnly
{
    public void Simple()
    {
        #region SendOnlyV5

        var configuration = new BusConfiguration();
        
        var bus = Bus.CreateSendOnly(configuration);

        #endregion
    }

}