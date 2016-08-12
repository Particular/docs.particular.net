Host name of the machine running each NServiceBus endpoint can be specified directly in the message-endpoint mapping configuration section by adding `@machine` suffix.

snippet:endpoint-mapping-msmq

When using MSMQ, if there is no `@machine` part, NServiceBus assumes the configured endpoint runs on a local machine.

MSMQ
