---
title: Throughput limitations when running without a license
summary: No limitations are enforced as of NServiceBus Version 4.5
tags: []
redirects:
 - nservicebus/licensing-limitations
---


## NServiceBus Version 4.5 and up

No limitations are enforced by NServiceBus if no license is found.


## NServiceBus Version 4.0 to NServiceBus Version 4.5

If running with no license the endpoint will restrict the maximum message throughput to 1 msg/s. Unlike Version 3 the number of threads is kept unlimited. You can read more on how to limit message throughput [here](/nservicebus/operations/tuning.md).


## NServiceBus Version 3

Version 3 will limit the number of worker threads used by the endpoint to 1.
