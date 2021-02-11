using NServiceBus;

class CustomSchemaAndCatalogForTheMainEndpoint
{
    public CustomSchemaAndCatalogForTheMainEndpoint()
    {
        #region 6to7-main-endpoint-custom-schema-and-catalog

        var endpointName = "EndpointName";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        var transport = new SqlServerTransport("connectionString");
        transport.SchemaAndCatalog.UseSchemaForQueue(endpointName, "schema");
        transport.SchemaAndCatalog.UseCatalogForQueue(endpointName, "catalog");

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}