## Multiple connection strings

include: multi-instance-connection-requirements

Connection strings for the remote endpoint can be configured via `app.config` convention. The endpoint-specific connection information can be discovered by reading the connection strings from the configuration file with `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention.

Given the following mappings:

snippet: sqlserver-multidb-messagemapping

and the following connection strings:

snippet: sqlserver-multidb-connectionstrings

The messages sent to the endpoint called `billing` will be dispatched to the database catalog `Billing` on the server instance `DbServerB`. Because the endpoint configuration isn't specified for `sales`, any messages sent to the `sales` endpoint will be dispatched to the default database catalog and database server instance. In this example that will be `MyDefaultDB` on server `DbServerA`.