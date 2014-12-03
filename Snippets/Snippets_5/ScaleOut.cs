using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOut

        var configuration = new BusConfiguration();

        configuration.ScaleOut().UseSingleBrokerQueue();
        //or
        configuration.ScaleOut().UseUniqueBrokerQueuePerMachine();

        #endregion
    }

}