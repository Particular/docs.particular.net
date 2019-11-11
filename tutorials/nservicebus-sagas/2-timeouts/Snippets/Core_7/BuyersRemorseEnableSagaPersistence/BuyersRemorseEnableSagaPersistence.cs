namespace Core_7.BuyersRemorseEnableSagaPersistence
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

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

}