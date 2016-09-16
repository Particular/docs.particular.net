## Transport Layer Security support

In Versions 3.2 and above, secure connections to the broker using Transport Layer Security (TLS) are supported. For information on how to configure TLS on the broker, refer to the [RabbitMQ documentation](http://www.rabbitmq.com/ssl.html). To enable TLS support, set the `UseTls` setting to `true` in the connection string. If the broker has been configured to require client authentication, a client certificate can be specified in the `CertPath` setting. If that certificate requires a password, it can be specified in the `CertPassphrase` setting.

An example connection string using these settings:

snippet:rabbitmq-connection-tls

NOTE: TLS 1.2 is required to establish a secure connection, so the broker must have TLS 1.2 enabled.