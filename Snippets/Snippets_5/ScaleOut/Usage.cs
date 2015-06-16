namespace Snippets5.ScaleOut
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ScaleOut

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.ScaleOut().UseSingleBrokerQueue();
            //or
            busConfiguration.ScaleOut().UseUniqueBrokerQueuePerMachine();

            #endregion
        }

    }
}