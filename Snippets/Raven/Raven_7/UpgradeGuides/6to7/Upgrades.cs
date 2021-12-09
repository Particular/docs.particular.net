using NServiceBus;
using System.Threading;

namespace Raven_7
{
    class Upgrades
    {
        public void OutboxDisableCleanup(EndpointConfiguration endpointConfiguration)
        {
            #region OutboxRavendBDisableCleanup6to7
            var outbox = endpointConfiguration.EnableOutbox();
            outbox.SetFrequencyToRunDeduplicationDataCleanup(Timeout.InfiniteTimeSpan);
            #endregion
        }

        void EnableClusterWideTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-cluster-wide-transactions-6to7

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.EnableClusterWideTransactions();

            #endregion
        }

        void OptimisticConcurrency(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-persistence-optimistic-concurrency-6to7

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            var sagas = persistence.Sagas();
            sagas.UseOptimisticLocking();

            #endregion
        }
    }
}
