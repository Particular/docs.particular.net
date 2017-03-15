In [*multi-catalog* and *multi-instance* modes](/nservicebus/sqlserver/deployment-options.md) additional configuration is required for proper message routing:

 * The sending endpoint needs the connection string of the receiving endpoint.
 * The subscribing endpoint needs the connection string of the publishing endpoint, in order to send subscription request.

Connection strings for the remote endpoint can be configured using following API

snippet:sqlserver-multidb-other-endpoint-connection

NOTE: The `address` parameter passed to the callback above is a transport address. It conforms to the `queue@[schema]` convention, e.g. could be equal to `MultiInstanceSender@[dbo]`.
