---
title: Rerouting Existing Timeouts
component: Raven
versions: '[3,)'
reviewed: 2019-05-30
tags:
 - Persistence
related:
 - nservicebus/endpoints/decommissioning-endpoints
redirects:
 - nservicebus/ravendb/reroute-existing-timeouts
---

include: dtc-warning

After moving an endpoint from one machine to another or changing an endpoints name, existing timeouts will need to be manually modified to end up in the new endpoint. To do that, follow these steps:

partial: fields
