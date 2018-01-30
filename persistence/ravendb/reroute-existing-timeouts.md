---
title: Rerouting Existing Timeouts
component: Raven
versions: '[3,)'
reviewed: 2017-08-21
tags:
 - Persistence
related:
 - nservicebus/endpoints/decommissioning-endpoints
redirects:
 - nservicebus/ravendb/reroute-existing-timeouts
---

include: dtc-warning

After moving an endpoint from one machine to another or changing endpoint name existing timeouts will need to be manually modified so that they could be delivered to new endpoint. In order to do that the following steps should be followed:

partial: fields
