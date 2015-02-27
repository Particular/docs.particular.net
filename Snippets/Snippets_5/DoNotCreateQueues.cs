using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueues

        BusConfiguration configuration = new BusConfiguration();

        configuration.DoNotCreateQueues();

        #endregion
    }

}