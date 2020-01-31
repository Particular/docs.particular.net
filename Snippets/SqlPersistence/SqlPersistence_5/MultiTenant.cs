using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class MultiTenant
{
    void WithHeaderName(EndpointConfiguration endpointConfiguration)
    {
        #region MultiTenantWithHeaderName 4.5
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.MultiTenantConnectionBuilder(tenantIdHeaderName: "TenantHeaderName",
            buildConnectionFromTenantData: tenantId =>
            {
                var connection = $@"Data Source=.\SqlExpress;Initial Catalog=DatabaseForTenant_{tenantId};Integrated Security=True";
                return new SqlConnection(connection);
            });
        #endregion
    }

    void WithFunc(EndpointConfiguration endpointConfiguration)
    {
        #region MultiTenantWithFunc 4.5
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.MultiTenantConnectionBuilder(captureTenantId: incomingMessage =>
            {
                if (incomingMessage.Headers.TryGetValue("NewTenantHeaderName", out var tenantId) || incomingMessage.Headers.TryGetValue("OldTenantHeaderName", out tenantId))
                {
                    return tenantId;
                }

                return null;
            },
            buildConnectionFromTenantData: tenantId =>
            {
                var connection = $@"Data Source=.\SqlExpress;Initial Catalog=DatabaseForTenant_{tenantId};Integrated Security=True";
                return new SqlConnection(connection);
            });
        #endregion
    }

    void DisablingOutboxCleanup(EndpointConfiguration endpointConfiguration)
    {
        #region DisableOutboxForMultiTenant 4.5
        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.DisableCleanup();
        #endregion
    }
}