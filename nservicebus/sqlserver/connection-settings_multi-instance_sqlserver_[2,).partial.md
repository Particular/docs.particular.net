In [*multi-catalog* and *multi-instance* modes](/nservicebus/sqlserver/deployment-options.md) additional configuration is required for proper message routing:

 * The sending endpoint needs the connection string of the receiving endpoint.
 * The subscribing endpoint needs the connection string of the publishing endpoint, in order to send subscription request.

Connection strings for the remote endpoint can be configured in several ways:

partial:multiple-connection-push


### Via the configuration API - Pull mode

The pull mode can be used when specific information is not available at configuration time. One can pass a function that will be used by the SQL Server transport to resolve connection information at runtime.

snippet:sqlserver-multidb-other-endpoint-connection-pull

Note that in Version 3 the `EnableLegacyMultiInstanceMode` method passes transport address parameter. Transport address conforms to the `endpoint_name@schema_name` convention, e.g. could be equal to `Samples.SqlServer.MultiInstanceSender@[dbo]`.


partial: multiple-appconfig