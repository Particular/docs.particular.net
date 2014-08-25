using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueuesV5

        var configuration = new BusConfiguration();

        configuration.DoNotCreateQueues();

        #endregion
    }

}