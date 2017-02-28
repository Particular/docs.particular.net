## Transport Layer Security support

Secure connections to the broker using [Transport Layer Security (TLS)](http://www.rabbitmq.com/ssl.html) are supported. To enable TLS support, set the `UseTls` setting to `true` in the connection string. If the broker has been configured to require client authentication, a client certificate must be specified in the `CertPath` setting. If that certificate requires a password, it must be specified in the `CertPassphrase` setting.

An example connection string using these settings:

snippet:rabbitmq-connection-tls

In Versions 4.3 and above, client certificates can be specified via code instead of using `CertPath` and `CertPassphrase`:

snippet:rabbitmq-config-client-certificates

NOTE: If a certificate is specified via the code API, the `CertPath` and `CertPassphrase` connection string settings are ignored.

WARN: TLS 1.2 must be enabled on the broker to establish a secure connection.