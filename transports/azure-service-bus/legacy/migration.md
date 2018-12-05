---
title: Migration off Endpoint-Oriented topology
summary: Migrating system off Endpoint-Oriented topology to Forwarding topology
component: ASB
versions: '[9.1,)'
tags:
- Azure
- Transport
reviewed: 2018-08-29
related:
 - transports/azure-service-bus/legacy/topologies
---

include: legacy-asb-warning

Moving from EOT to F topology - why? To eventually move to the new transport.
Why not to move directly from EOT? Incompatible with the new transport.
What is compatible? F topology. (link: transports/azure-service-bus/compatibility).


## Side-by-side migration

describe steps


## Moving off the legacy transport

What's needed to move to the new transport? transports/azure-service-bus/compatibility


## Finalizing migration (cleanup stage)

What needs to be done to remove parts of old topology that is no longer utilized


## Nerdy section - how it works at the high livel

High level description of how migration operates. Similar to transports/rabbitmq/delayed-delivery.md#how-it-works