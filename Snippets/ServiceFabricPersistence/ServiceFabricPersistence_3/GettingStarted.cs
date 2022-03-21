using Microsoft.ServiceFabric.Data;
using NServiceBus;
using NServiceBus.Persistence.ServiceFabric;
using System;

class GettingStarted
{
    void GettingStartedUsage(IReliableStateManager reliableStateManager, EndpointConfiguration endpointConfiguration)
    {
        #region ServiceFabricPersistenceConfiguration
        var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();
        persistence.StateManager(reliableStateManager);
        #endregion
    }

    void TransactionTimeoutUsage(IReliableStateManager reliableStateManager, EndpointConfiguration endpointConfiguration)
    {
        #region ServiceFabricPersistenceConfigurationTransactionTimeout
        var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();
        persistence.StateManager(reliableStateManager, transactionTimeout: TimeSpan.FromSeconds(10));
        #endregion
    }
}