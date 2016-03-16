---
title: Throughput limitations when running without a license
summary: No limitations are enforced as of NServiceBus Version 4.5
reviewed: 2016-03-17
redirects:
 - nservicebus/licensing-limitations
---


## NServiceBus Version 4.5 and above

No limitations are enforced by NServiceBus if no license is found.


## NServiceBus Version 4.0 to NServiceBus Version 4.5

If running with no license the endpoint will restrict the [maximum message throughput](/nservicebus/operations/tuning.md) to 1 msg/s. Unlike Version 3 the number of threads is kept unlimited.


## NServiceBus Version 3

Version 3 will limit the number of worker threads used by the endpoint to 1.