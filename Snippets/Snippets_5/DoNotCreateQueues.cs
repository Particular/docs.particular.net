using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueues

        var configuration = new BusConfiguration();

        configuration.DoNotCreateQueues();

        #endregion
    }

}