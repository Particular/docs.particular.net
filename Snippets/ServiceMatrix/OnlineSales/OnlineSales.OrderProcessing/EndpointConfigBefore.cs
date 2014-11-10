using System;
using NServiceBus;
using System.Diagnostics;

namespace OnlineSales.OrderProcessing
{
    // This class isn't used for anything other than to show a snippet of the persistence configuration BEFORE modifying it for RavenDB
    public class EndpointConfigBefore
    {
        public void Customize(BusConfiguration configuration)
        {
            #region ServiceMatrix.OnlineSales.OrderProcessing.EndpointConfig.before
            // For production use, please select a durable persistence.
            // To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
            // To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();
            if (Debugger.IsAttached)
            {
                configuration.UsePersistence<InMemoryPersistence>();
            }
            #endregion
        }
    }
}
