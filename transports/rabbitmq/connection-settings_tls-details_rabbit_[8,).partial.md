## Transport Layer Security support

Secure connections to the broker using [Transport Layer Security (TLS)](https://www.rabbitmq.com/ssl.html) are supported. To enable TLS support, set the `UseTls` setting to `true` in the connection string:

snippet: rabbitmq-connection-tls

WARNING: TLS 1.2 must be enabled on the broker to establish a secure connection.


### Client authentication

If the broker has been configured to require client authentication, a client certificate must be specified in the `CertPath` setting. If that certificate requires a password, it must be specified in the `CertPassphrase` setting.

Here is a sample connection string using these settings:

snippet: rabbitmq-connection-client-auth

Client certificates can also be specified via code instead of using the connection string:

snippet: rabbitmq-client-certificate

NOTE: If a certificate is specified via either code API, the `CertPath` and `CertPassphrase` connection string settings are ignored.


### Remote certificate validation 

By default, the RabbitMQ client will refuse to connect to the broker if the remote server certificate is invalid. In NServiceBus.RabbitMQ versions 4.4 and above, this validation can be disabled with the following setting:

snippet: rabbitmq-disable-remote-certificate-validation


### External authentication

By default, the broker requires a username and password to authenticate the client, but it can be configured to use other external authentication mechanisms. If the broker requires an external authentication mechanism, the client can be configured to use it with the following setting:

snippet: rabbitmq-external-auth-mechanism
