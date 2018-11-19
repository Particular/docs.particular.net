
NOTE: When using instance mapping, the settings will have no effect on **audit/error** queue and **publish/subscribe** (except for subscription messages, subscription messages will be sent to all listed instances)

Host name of the machine running each endpoint can be specified directly in the message-endpoint mapping configuration section by adding `@machine` suffix.

snippet: endpoint-mapping-msmq

When using MSMQ, if there is no `@machine` part, NServiceBus assumes the configured endpoint runs on a local machine.
