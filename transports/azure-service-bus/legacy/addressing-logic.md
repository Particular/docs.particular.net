---
title: Addressing Logic
summary: How to map logical endpoint names to physical Azure Service Bus addresses
component: ASB
reviewed: 2020-02-18
redirects:
 - nservicebus/azure-service-bus/addressing-logic
 - transports/azure-service-bus/addressing-logic
---

include: legacy-asb-warning


One of the responsibilities of the Azure Service Bus transport is determining the names and physical location of entities in the underlying physical infrastructure. This is achieved by turning logical endpoint names into physical addresses of the Azure Service Bus entities, which is called *Physical Addressing Logic*.

partial: body