using NServiceBus;


public class SendOnly
{
    public void Simple()
    {
        #region SendOnly

        BusConfiguration configuration = new BusConfiguration();
        
        ISendOnlyBus bus = Bus.CreateSendOnly(configuration);

        #endregion
    }

}