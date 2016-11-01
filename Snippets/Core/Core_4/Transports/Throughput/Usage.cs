namespace Core4.Transports.Throughput
{
    using NServiceBus.Unicast;

    class Usage
    {

        void ReadTuning(UnicastBus unicastBus)
        {
            #region ReadTuning

            var transport = unicastBus.Transport;
            var messageThroughputPerSecond = transport.MaximumMessageThroughputPerSecond;
            var maximumConcurrencyLevel = transport.MaximumConcurrencyLevel;

            #endregion
        }

        void ChangeTuning(UnicastBus unicastBus)
        {
            #region ChangeTuning

            var transport = unicastBus.Transport;
            transport.ChangeMaximumMessageThroughputPerSecond(10);
            transport.ChangeMaximumConcurrencyLevel(5);

            #endregion

        }

    }
}