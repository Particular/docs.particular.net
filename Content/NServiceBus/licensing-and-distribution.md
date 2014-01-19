---
title: Licensing limitations when using the Distributor
summary: Basic licenses are limited to two worker nodes.
originalUrl: http://www.particular.net/articles/licensing-and-distribution
tags: []
createdDate: 2013-05-22T08:42:07Z
modifiedDate: 2013-11-25T06:11:25Z
authors: []
reviewers: []
contributors: []
---

Basic licenses (default, express, Basic-2, 4, etc.) allow you to run your distributor with two worker nodes.

<span style="font-size: 14.399999618530273px;">If your
</span>[NServiceBus generic host](the-nservicebus-host.md)<span style="font-size: 14.399999618530273px;"> is running with the NServiceBus.Master profile or if you are self hosting and the bus is initialized with RunDistributor() then only one additional worker can register with the distributor. </span>Read more about the [Distributor and worker nodes](particular.net/articles/load-balancing-with-the-distributor).

Purchase of a standard license (or use of a time-limited trial license) removes the limitation of two worker nodes. Read more about
[licensing](particular.net/licensing).

