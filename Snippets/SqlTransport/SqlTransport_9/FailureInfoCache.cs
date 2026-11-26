using System;
using NServiceBus;

class FailureInfoCache
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        var transport = new SqlServerTransport("");

        #region FailureInfoCache

        transport.FailureInfoCache.MaxEntries = 1_000;
        transport.FailureInfoCache.ExpirationPeriod = TimeSpan.FromMinutes(30);

        #endregion
    }
}
