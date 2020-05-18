
The host name of the machine running each endpoint can be specified directly in the message-endpoint mapping configuration section by adding a `@machine` suffix.

snippet: endpoint-mapping-msmq

When using MSMQ, if there is no `@machine` suffix, NServiceBus assumes the configured endpoint runs on a local machine.
