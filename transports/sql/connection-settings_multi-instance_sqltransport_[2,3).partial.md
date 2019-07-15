## Multiple connection strings

include: multi-instance-connection-requirements

Connection strings for the remote endpoint can be configured in several ways:


### Via the configuration API - push mode

In push mode the entire collection of endpoint connection information objects is passed during configuration time.

snippet: sqlserver-multidb-other-endpoint-connection-push


### Via the configuration API - pull mode

Pull mode can be used when specific information is not available at configuration time. A function can be passed in that will be used by the SQL Server transport to resolve connection information at runtime.

snippet: sqlserver-multidb-other-endpoint-connection-pull

Note that in version 3 of the transport the `EnableLegacyMultiInstanceMode` method passes a transport address parameter. The transport address conforms to the `endpoint_name@schema_name` convention, e.g. `Samples.SqlServer.MultiInstanceSender@[dbo]`.


### Via the App.Config

The endpoint-specific connection information can be discovered by reading the connection strings from the configuration file with the `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention.


Given the following mappings:

snippet: sqlserver-multidb-messagemapping

and the following connection strings:

snippet: sqlserver-multidb-connectionstrings

The messages sent to the endpoint called `billing` are dispatched to the database catalog `Billing` on the server instance `DbServerB`. Because the endpoint configuration isn't specified for `sales`, any messages sent to the `sales` endpoint are dispatched to the default database catalog and database server instance. In this example this is `MyDefaultDB` on server `DbServerA`.
