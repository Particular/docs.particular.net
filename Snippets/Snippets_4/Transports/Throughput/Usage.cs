namespace Snippets4.Transports.Throughput
{
    using NServiceBus.Unicast;

    class Usage
    {

        Usage(UnicastBus unicastBus)
        {
            #region ChangeTuning
            unicastBus.Transport.ChangeMaximumMessageThroughputPerSecond(10);
            unicastBus.Transport.ChangeMaximumConcurrencyLevel(5);
            #endregion

            #region ReadTuning
            int messageThroughputPerSecond = unicastBus.Transport.MaximumMessageThroughputPerSecond;
            int maximumConcurrencyLevel = unicastBus.Transport.MaximumConcurrencyLevel;

            #endregion
        }

    }
}