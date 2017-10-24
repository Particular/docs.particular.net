MSMQ is the default transport used by NServiceBus. Therefore when using MSMQ the transport configuration for the endpoint need not be specified. However if the connection string needs to be configured via code, it can be done as shown. In the example below the dead letter queue configuration is turned off.

snippet: msmq-config-basic

INFO: When using MSMQ as the transport, the [error queue configuration](/nservicebus/recoverability/configure-error-handling.md) must also be specified.
