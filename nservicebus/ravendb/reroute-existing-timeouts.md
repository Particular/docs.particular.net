---
title: Rerouting Existing Timeouts
component: Raven
reviewed: 2016-09-22
tags:
 - RavenDB
 - Persistence
 - Timeouts
related:
 - nservicebus/endpoints/decommissioning-endpoints
---

## Rerouting existing timeouts 

After moving an endpoint from one machine to another or changing endpoint name existing timeouts will need to be manually modified so that they could be delivered to new endpoint. In order to do that the following steps should be followed:

partial: fields