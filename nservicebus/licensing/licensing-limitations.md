---
title: Throughput limitations when running without a license
reviewed: 2016-08-11
redirects:
 - nservicebus/licensing-limitations
---


## Versions 4.5 and above

No limitations are enforced by NServiceBus if no license is found.


## Versions 4.0 to 4.5

If running with no license the endpoint will restrict the [maximum message throughput](/nservicebus/operations/tuning.md) to 1 msg/s. Unlike Version 3 the number of threads is kept unlimited.


## Version 3

Version 3 will limit the number of worker threads used by the endpoint to 1.
