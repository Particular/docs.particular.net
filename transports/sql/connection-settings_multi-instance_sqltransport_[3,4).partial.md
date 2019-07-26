## Multiple connection strings

NOTE: Support for multiple connection strings has been removed in version 4 of the transport.

include: multi-instance-connection-requirements

Connection strings for the remote endpoint is configured using following API

snippet: sqlserver-multidb-other-endpoint-connection

NOTE: The `address` parameter passed to the callback above is a transport address. It conforms to the `queue@[schema]` convention, e.g. `MultiInstanceSender@[dbo]`.
