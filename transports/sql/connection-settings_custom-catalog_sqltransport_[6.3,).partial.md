## Custom database catalog

The SQL Server transport uses the catalog specified in the connection string by default. This can be overridden with the `DefaultCatalog` method:

snippet: sqlserver-override-default-catalog

By overriding the catalog used by the SQL Server transport, the system's business data can be kept in a separate catalog than the transport data, allowing each database to be managed and maintained with different policies.