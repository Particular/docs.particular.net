---
title: Decommissioning Endpoints
summary: Describes the process of permanently shutting down endpoints
reviewed: 2025-03-27
---

Endpoint decommissioning is the process of permanently shutting down an endpoint instance. This can be done for various reasons, such as:

 * Dividing the endpoint's responsibility among other endpoints
 * Scaling down endpoint instances
 * Other (like removing certain functionality from the system)

When decommissioning endpoints, several aspects should be considered:

 1. Routing should be adjusted to make sure that no new messages will be delivered to that endpoint.
 1. All messages should be processed from the endpoint's queue.
 1. [Timeouts should be rerouted to other endpoint instances](/persistence/ravendb/reroute-existing-timeouts.md), _if applicable_.
 1. If any errors are discovered in the ServicePulse/ServiceControl tools, [redirect functionality](/servicepulse/redirect.md) can be used to reroute messages to another endpoint instance.
