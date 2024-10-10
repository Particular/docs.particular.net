using NServiceBus;

namespace Core_9.BuyersRemorseEnableSagaPersistence;

class Program
{
    void BuyersRemorseEnableSagaPersistence()
    {
        EndpointConfiguration endpointConfiguration = null;

        #region BuyersRemorseEnableSagaPersistence
        var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
        #endregion
    }
}