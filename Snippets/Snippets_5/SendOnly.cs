using NServiceBus;


public class SendOnly
{
    public void Simple()
    {
        #region SendOnlyV5

        var configuration = new BusConfiguration();
        
        configuration.SendOnly();

        var bus = Bus.Create(configuration);

        #endregion
    }

}