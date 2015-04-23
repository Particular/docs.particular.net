using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOut

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.ScaleOut().UseSingleBrokerQueue();
        //or
        busConfiguration.ScaleOut().UseUniqueBrokerQueuePerMachine();

        #endregion
    }

}