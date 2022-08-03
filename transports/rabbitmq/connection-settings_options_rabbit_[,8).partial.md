### CertPath

The file path to the client authentication certificate when using [TLS](#transport-layer-security-support).

### CertPassphrase

The password for the client authentication certificate specified in `CertPath`

### RequestedHeartbeat

The interval for heartbeats between the endpoint and the broker.

Default: `60` seconds

### RetryDelay

The time to wait before trying to reconnect to the broker if the connection is lost.

Type: `System.TimeSpan`

Default: `00:00:10` (10 seconds)