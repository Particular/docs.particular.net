﻿using NServiceBus;

namespace Raven_5
{
    class MultiTenantSupport
    {
        void UseSetMessageToDatabaseMappingConvention(EndpointConfiguration endpointConfiguration)
        {
            #region multi-tenant-support
            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.SetMessageToDatabaseMappingConvention(headers =>
            {
                //based on incoming message headers select the correct RavenDB Database
                return "selected-database-name";
            });
            #endregion
        }
    }
}
