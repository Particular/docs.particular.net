---
title: Rerouting Existing Timeouts
summary: How to reroute existing timeouts in the Raven persister when an endpoint is moved
component: Raven
reviewed: 2026-06-09
related:
 - nservicebus/endpoints/decommissioning-endpoints
redirects:
 - nservicebus/ravendb/reroute-existing-timeouts
---

After moving an endpoint from one machine to another or changing an endpoint's name, existing timeouts must be manually modified to end up in the new endpoint. To do that, follow these steps:

* Update the following values in the Timeout Data documents in the RavenDB persister with the new endpoint name or updated machine name:
  * Destination
  * OwningTimeoutManager (contains endpoint name only)
  * Headers:
    * NServiceBus.ReplyToAddress
    * NServiceBus.Timeout.RouteExpiredTimeoutTo
    * NServiceBus.OriginatingEndpoint
* After the above changes are made, restart the endpoint to process the timeouts.
