---
title: Decommissioning Endpoints
summary: Describes the process of permanently shutting down endpoints
reviewed: 2026-10-21
---

Endpoint decommissioning is the process of permanently shutting down an endpoint instance. This can be done for various reasons, such as:

- Dividing the endpoint's responsibility among other endpoints
- Scaling down endpoint instances
- Other (like removing certain functionality from the system)

When decommissioning endpoints, several aspects should be considered:

1. Routing should be adjusted to make sure that no new messages will be delivered to that endpoint.
1. All messages in the endpoint's queue should be processed.
1. Subscriptions for the events the endpoint receives should be [removed](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#decomissioning-event-handlers).
1. In NServiceBus version 7 and earlier, endpoints that use the [timeout manager](/nservicebus/messaging/timeout-manager.md) store timeouts that [should be rerouted to other endpoint instances](/persistence/ravendb/reroute-existing-timeouts.md), _if applicable_. In version 8 and later, the timeout manager is removed, and any remaining timeouts can be migrated using the [timeouts migration tool](/nservicebus/tools/migrate-to-native-delivery.md).
1. If messages for the decommissioned endpoint fail, use [ServicePulse redirects](/servicepulse/redirect.md) to retry them against another endpoint.
1. Remove the decommissioned endpoint instances from ServicePulse. See [removing endpoint instances](/monitoring/heartbeats/in-servicepulse.md#instances-removing-endpoint-instances).
