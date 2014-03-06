---
title: Licensing limitations when using the Distributor
summary: Basic licenses are limited to two worker nodes.
tags: []
---

Basic licenses (default, express, Basic-2, 4, etc.) allow you to run your distributor with two worker nodes.

If your [NServiceBus generic host](the-nservicebus-host.md) is running with the NServiceBus.Master profile or if you are self hosting and the bus is initialized with RunDistributor() then only one additional worker can register with the distributor. Read more about the [Distributor and worker nodes](load-balancing-with-the-distributor).

Purchase of a standard license (or use of a time-limited trial license) removes the limitation of two worker nodes. Read more about [licensing](http://particular.net/licensing).

