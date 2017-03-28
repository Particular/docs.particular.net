The Gateway has its own retry mechanism. It will retry failed messages 4 times by default, increasing the delay by 60 seconds each time as follows:

Retry | Delay
---- | ----
1 | 60 seconds
2 | 120 seconds
3 | 180 seconds
4 | 240 seconds

The number of retries and the time to increase between retries can be configured using configuration API:

snippet: GatewayDefaultRetryPolicyConfiguration

The default retry policy can be replaced by implementing a `Func<IncomingMessage, Exception, int, TimeSpan>` to calculate the delay for each retry:

snippet: GatewayCustomRetryPolicyConfiguration

The provided example shows the built-in default retry policy.

Custom retry policies should eventually give up retrying, otherwise the message could get stuck in a loop being retried forever. To discontinue retries return `TimeSpan.MinValue` from the custom retry policy. The message will be then be handled by [recoverability](/nservicebus/recoverability/).

WARNING: The recoverability mechanism built into the Gateway does not roll back the [receive transaction](/nservicebus/messaging/) or any ambient transactions when sending a message to another site fails. Any custom recoverability policy cannot rely on an ambient transaction being rolled back.

To disable retries in the gateway use the `DisableRetries` setting:

snippet: GatewayDisableRetriesConfiguration

When retries are disabled, any messages that fail to be sent to another site will be immediately routed to the configured error queue.
