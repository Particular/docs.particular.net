namespace Raven_6
{
    using System;
    using NServiceBus;

    class SagasConfiguration
    {
        void UsePersistence(EndpointConfiguration endpointConfiguration)
        {
            #region ravendb-cluster-wide-transactions
            var endpointConfig = endpointConfiguration.UsePersistence<RavenDBPersistence>()
            endpointConfig.UseClusterWideTransactions();
            #endregion
        }
    }
}