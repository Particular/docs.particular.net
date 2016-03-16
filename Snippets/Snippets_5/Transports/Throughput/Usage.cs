namespace Snippets5.Transports.Throughput
{
    using NServiceBus.Unicast;

    public class Usage
    {

        public Usage()
        {
            UnicastBus unicastBus = null;

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