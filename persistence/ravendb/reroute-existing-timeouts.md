---
title: Rerouting Existing Timeouts
summary: How to reroute existing timeouts in the Raven persister when an endpoint is moved
component: Raven
versions: '[3,)'
reviewed: 2019-06-10
related:
 - nservicebus/endpoints/decommissioning-endpoints
redirects:
 - nservicebus/ravendb/reroute-existing-timeouts
---

include: dtc-warning

include: cluster-configuration-warning

After moving an endpoint from one machine to another or changing an endpoint's name, existing timeouts must be manually modified to end up in the new endpoint. To do that, follow these steps:

partial: fields