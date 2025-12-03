---
title: Azure Functions (Isolated Worker) Upgrade Version 5 to 6
summary: How to upgrade Azure Functions (Isolated Worker) with Service Bus from version 5 to 6
component: ASBFunctionsWorker
reviewed: 2025-03-14
isUpgradeGuide: true
---

## Default topology has changed

Version 6 of the functions integration now uses version 5 of the Azure Service Bus Transport, which introduced a new [default topology](/transports/azure-service-bus/topology.md). See the [Azure Service Bus Transport Upgrade Version 4 to 5](/transports/upgrades/asbs-4to5.md) for more details.

The default topology options can be overridden by adding settings to the [application configuration](/nservicebus/hosting/azure-functions-service-bus/#preparing-the-azure-service-bus-namespace-topology-configuration).