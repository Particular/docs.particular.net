namespace Core5.Transports.Throughput
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
            var messageThroughputPerSecond = unicastBus.Transport.MaximumMessageThroughputPerSecond;
            var maximumConcurrencyLevel = unicastBus.Transport.MaximumConcurrencyLevel;

            #endregion
        }

    }
}