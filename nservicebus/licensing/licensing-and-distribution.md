---
title: Licensing limitations when using the Distributor
summary: Basic licenses are limited to two worker nodes.
tags:
- Distributor
- Licensing
redirects:
 - nservicebus/licensing-and-distribution
---

**NServiceBus Version 4.5 and above**

No limitations is enforced by the distributor

**Up to NServiceBus Version 4.5**

Basic licenses (default, express, Basic-2, 4, etc.) allow you to run your distributor with two worker nodes.

If your [NServiceBus generic host](/nservicebus/hosting/nservicebus-host/) is running with the NServiceBus.Master profile or if you are self hosting and the bus is initialized with RunDistributor() then only one additional worker can register with the distributor. Read more about the [Distributor and worker nodes](/nservicebus/scalability-and-ha/distributor/).

Purchase of a standard license (or use of a time-limited trial license) removes the limitation of two worker nodes. Read more about [licensing](http://particular.net/licensing).

