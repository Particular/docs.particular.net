using NServiceBus;
using System.Threading;

namespace Raven_7
{
    class OutboxUpgrade
    {
        public void OutboxDisableCleanup(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBDisableCleanup
            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);
            #endregion
        }
    }
}
