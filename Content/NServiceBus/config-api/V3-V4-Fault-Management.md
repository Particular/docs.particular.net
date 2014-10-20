---
title: Configuration API Fault Management in V3 and V4
summary: Configuration API Fault Management in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

NServiceBus [manages fault](msmqtransportconfig). To activate the fault manager, call the `MessageForwardingInCaseOfFault()` method.

	//TODO: http://andreasohlund.net/2012/09/26/disabling-second-level-retries-for-specific-exceptions/ ?