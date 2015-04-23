using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueues

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.DoNotCreateQueues();

        #endregion
    }

}