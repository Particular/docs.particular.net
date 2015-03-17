using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOut

        BusConfiguration configuration = new BusConfiguration();

        configuration.ScaleOut().UseSingleBrokerQueue();
        //or
        configuration.ScaleOut().UseUniqueBrokerQueuePerMachine();

        #endregion
    }

}