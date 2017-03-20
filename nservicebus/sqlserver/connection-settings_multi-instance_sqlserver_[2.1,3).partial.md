include: multi-instance-connection-requirements

Connection strings for the remote endpoint can be configured in several ways:


### Via the configuration API - Push mode

In push mode the whole collection of endpoint connection information objects is passed during configuration time.

snippet: sqlserver-multidb-other-endpoint-connection-push


### Via the configuration API - Pull mode

Pull mode can be used when specific information is not available at configuration time. One can pass a function that will be used by the SQL Server transport to resolve connection information at runtime.

snippet: sqlserver-multidb-other-endpoint-connection-pull

Note that in Version 3 the `EnableLegacyMultiInstanceMode` method passes transport address parameter. Transport address conforms to the `endpoint_name@schema_name` convention, e.g. could be equal to `Samples.SqlServer.MultiInstanceSender@[dbo]`.


### Via the App.Config

The endpoint-specific connection information can be discovered by reading the connection strings from the configuration file with `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention.


Given the following mappings:

snippet: sqlserver-multidb-messagemapping

and the following connection strings:

snippet: sqlserver-multidb-connectionstrings

The messages sent to the endpoint called `billing` will be dispatched to the database catalog `Billing` on the server instance `DbServerB`. Because the endpoint configuration isn't specified for `sales`, any messages sent to the `sales` endpoint will be dispatched to the default database catalog and database server instance. In this example that will be `MyDefaultDB` on server `DbServerA`.
