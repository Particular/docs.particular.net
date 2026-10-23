
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

Subscribing to an event creates a subscription on each topic the event is mapped to. Creating a subscription requires `Manage` rights, even when the subscription already exists. Without them, the subscribe operation fails and an error is logged, but the endpoint still starts. If the subscription already exists, events continue to arrive through it. With [filtered routing](/transports/azure-service-bus/topology.md#subscription-rule-matching-advanced-multiplexing-strategies-filtering-within-a-multiplexed-topic), the filter rule for each event must also already exist, because the transport doesn't add rules when creating the subscription fails. If the subscription or rule doesn't exist, the endpoint doesn't receive the events that depend on it. Events published before the subscription or rule is created are never delivered to the endpoint.

With filtered routing, unsubscribing from an event deletes only the event's filter rule, which doesn't require `Manage` rights. Otherwise, unsubscribing deletes the subscription. Without `Manage` rights, the transport leaves the subscription in place and logs the failure at `Info` level. Events published to that topic continue to arrive in the endpoint's input queue and are processed as before. If the endpoint is stopped or decommissioned, these events accumulate in its input queue. To stop the events, remove the subscription using [operational scripting](/transports/azure-service-bus/operational-scripting.md).
#end-if