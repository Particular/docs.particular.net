include: multi-instance-connection-requirements

Connection strings for the remote endpoint can be configured using following API

snippet: sqlserver-multidb-other-endpoint-connection

NOTE: The `address` parameter passed to the callback above is a transport address. It conforms to the `queue@[schema]` convention, e.g. could be equal to `MultiInstanceSender@[dbo]`.
