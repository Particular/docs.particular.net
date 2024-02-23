## Registering multiple publishers for the same event

The messaging bridge will not allow registering multiple publishers for the same event according to the [events should be published by the logical owner](/nservicebus/messaging/messages-events-commands.md) convention.

To allow this, use the [NServiceBus.MessagingBridge version 2.1 or above](configuration.md#registering-multiple-publishers-for-the-same-event) and disable best practices enforcement:

snippet: do-not-enforce-best-practices

If the best practices are not enforced the bridge will log the following warning:

> The following subscriptions with multiple registered publishers are ignored as best practices are not enforced:
