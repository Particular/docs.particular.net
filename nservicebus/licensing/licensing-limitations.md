---
title: Throughput limitations when running without a license
summary: As of v4.5 no limitations are enforces
tags: []
redirects:
 - nservicebus/licensing-limitations
---

**NServiceBus V4.5 and up**

No limitations is enforced by NServiceBus if no license is found.

**NServiceBus V4.0 to NServiceBus V4.5**

If running with no license the endpoint will restrict the maximum message throughput to 1 msg/s. Unlike v3 the number of threads is keep unlimited. You can read more on how to limit message throughput [here](/nservicebus/operations/reducing-throughput.md).

**NServiceBus V3**

Version 3 will limit the number of worker threads used by the endpoint to 1