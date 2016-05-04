---
title: Upgrading the Distributor and Workers
summary: The Distributor and Workers need to be upgraded together or in a specific order.
tags:
- Scalability
- Distributor
---

Upgrading to a newer version of NServiceBus when using the Distributor should be done in one of two ways to avoid incompatibility between Distributor and Workers.

The two supported upgrade paths are:

 - Offline approach
 - Online approach


## Offline approach

This is the most straight forward scenario, but it requires to take both the Distributor and Workers offline at the same time.

The procedure is as follows:

 1. Take Workers offline
 1. Take Distributor offline
 1. Upgrade code for Workers to newer version of NServiceBus
 1. Upgrade code for the Distributor newer version of NServiceBus
 1. Restart Distributor
 1. Restart Workers

Load balancing will not be done by the Distributor and messages will not be handled by the Workers during the upgrade.


## Online approach

This approach does not require that both Workers and the Distributor are taken offline at the same time, hence reducing the service disruption during the upgrade. To make this possible it is important that the Workers are upgraded before the Distributor. The Distributor is explicitly designed to be compatible with Workers from both older and newer versions of NServiceBus, so there is no issue running different versions of Workers on the same Distributor at the same time.

The procedure is as follows:

 1. Take one Worker offline
 1. Upgrade code for Worker to a newer version of NServiceBus
 1. Restart Worker
 1. Repeat for all other Workers
 1. Take Distributor offline
 1. Upgrade code for the Distributor to a newer version of NServiceBus
 1. Restart Distributor

With this approach only one Worker needs to be offline at a time and messages are processed by other available Workers during the entire upgrade. Service disruption should be minimal.