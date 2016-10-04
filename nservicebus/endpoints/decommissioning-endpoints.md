---
title: Decommissioning Endpoints
summary: Describe the process of decommissioning endpoints.
reviewed: 2016-10-04
tags:
- Endpoint
- Endpoint Instance
related:
 - samples/endpoint-configuration

---

Endpoint decommission is a process of permanent endpoint instance shut down. It can happen for various reasons, such as:
 - Dividing endpoint responsibility into a few other endpoints
 - Scaling down endpoint instances
 - Other (like removing certain functionality from the system)

When performing endpoint decommission the following aspects should be considered:
1. Routing should be adjusted to make sure that no new messages will be delivered to that endpoint
2. Make sure that endpoint processed all messages from it's queue
3. [Timeouts are rerouted to other endpoint instances](/nservicebus/ravendb/reroute-existing-timeouts.md)
4. If any errors are discovered in the Service Pulse/Service Control tools a [redirect functionality](/servicepulse/redirect.md) can be used to reroute messages to another endpoint instance.