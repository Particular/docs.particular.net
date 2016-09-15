### UseTls

Indicates if the connection to the broker should be secured with [TLS](#connection-string-options-transport-layer-security-support).

Default: `false`

Versions: 3.2 and above


### CertPath

The file path to the client authentication certificate when using [TLS](#connection-string-options-transport-layer-security-support).

Versions: 3.2 and above


### CertPassphrase

The password for the client authentication certificate specified in `CertPath`

Versions: 3.2 and above


### Transport Layer Security support

In Versions 3.2 and above, the RabbitMQ transport supports creating secure connections to the broker using Transport Layer Security (TLS). For information on how to configure TLS on the RabbitMQ broker, refer to the [RabbitMQ documentation](http://www.rabbitmq.com/ssl.html). To enable TLS support, set the `UseTls` setting to `true` in the connection string. If the RabbitMQ broker has been configured to require client authentication, a client certificate can be specified in the `CertPath` setting. If that certificate requires a password, it can be specified in the `CertPassphrase` setting.

An example connection string using these settings:

snippet:rabbitmq-connection-tls

NOTE: The RabbitMQ transport requires TLS 1.2 to establish a secure connection, so the broker must have TLS 1.2 enabled.
