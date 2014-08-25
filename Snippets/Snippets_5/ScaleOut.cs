using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOutV5

        var configuration = new BusConfiguration();

        configuration.ScaleOut().UseSingleBrokerQueue();
        //or
        configuration.ScaleOut().UseUniqueBrokerQueuePerMachine();

        #endregion
    }

}