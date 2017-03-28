## Transport Layer Security support

In Versions 3.2 and above, secure connections to the broker using [Transport Layer Security (TLS)](http://www.rabbitmq.com/ssl.html) are supported. To enable TLS support, set the `UseTls` setting to `true` in the connection string. If the broker has been configured to require client authentication, a client certificate must be specified in the `CertPath` setting. If that certificate requires a password, it must be specified in the `CertPassphrase` setting.

An example connection string using these settings:

snippet: rabbitmq-connection-tls

WARNING: TLS 1.2 must be enabled on the broker to establish a secure connection.
