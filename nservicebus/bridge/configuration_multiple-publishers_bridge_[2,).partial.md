The messaging bridge will not allow registering multiple publishers for the same event according to the [events should be published by the logical owner](/nservicebus/messaging/messages-events-commands.md) convention, unless best practices enforcement is explicitly disabled:

snippet: do-not-enforce-best-practices

If the best practices are not enforced the bridge will log the following warning:

> The following subscriptions with multiple registered publishers are ignored as best practices are not enforced: