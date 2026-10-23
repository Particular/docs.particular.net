
### Access rights

By default, the transport requires elevated privileges to manage namespace entities at runtime. If using a [shared access policy](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-sas), make sure to include `Manage` rights or the [Azure Service Bus Data Owner](https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#azure-service-bus-data-owner) role if authenticating using [Managed Identities](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity).

To avoid running with elevated privileges:

- Make sure that [installers are not configured to run](/nservicebus/operations/installers.md)
- Use [operational scripting](/transports/azure-service-bus/operational-scripting.md) to provision entities (queues, topics, and subscriptions)
#if-version [,3)
- [Turn off automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#disabling-auto-subscription)
#end-if
#if-version [5,)
- [Turn off automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#disabling-auto-subscription)

Subscribing to an event creates a subscription on each topic the event is mapped to. Creating a subscription requires `Manage` rights, even when the subscription already exists. Without them, the subscribe operation fails and an error is logged. The endpoint still starts, and events continue to arrive through the existing subscription.

If unsubscribing from an event requires deleting a subscription and the endpoint doesn't have `Manage` rights, the transport leaves the subscription in place and logs the failure at `Info` level. Events published to that topic continue to arrive in the endpoint's input queue until the subscription is removed using [operational scripting](/transports/azure-service-bus/operational-scripting.md).
#end-if