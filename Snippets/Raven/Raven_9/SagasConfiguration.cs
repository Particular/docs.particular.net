﻿using System;
using NServiceBus;

class SagasConfiguration
{
    void SetPessimisticLeaseLockAcquisitionMaximumRefreshDelay(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-sagas-pessimisticLeaseLockAcquisitionMaximumRefreshDelay
        var sagasConfig = endpointConfiguration.UsePersistence<RavenDBPersistence>()
            .Sagas();
        sagasConfig.SetPessimisticLeaseLockAcquisitionMaximumRefreshDelay(TimeSpan.FromMilliseconds(500));
        #endregion
    }
    void SetPessimisticLeaseLockAcquisitionTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-sagas-setPessimisticLeaseLockAcquisitionTimeout
        var sagasConfig = endpointConfiguration.UsePersistence<RavenDBPersistence>()
            .Sagas();
        sagasConfig.SetPessimisticLeaseLockAcquisitionTimeout(TimeSpan.FromSeconds(15));
        #endregion
    }
    void SetPessimisticLeaseLockTime(EndpointConfiguration endpointConfiguration)
    {
        #region ravendb-sagas-setPessimisticLeaseLockTime
        var sagasConfig = endpointConfiguration.UsePersistence<RavenDBPersistence>()
            .Sagas();
        sagasConfig.SetPessimisticLeaseLockTime(TimeSpan.FromMinutes(2));
        #endregion
    }
}