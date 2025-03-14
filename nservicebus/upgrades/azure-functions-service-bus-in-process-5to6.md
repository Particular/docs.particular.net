---
title: Azure Functions (in-process) for Service Bus Upgrade Version 5 to 6
summary: How to upgrade Azure Functions (in-process) with Azure Service Bus from version 5 to version 6
component: ASBFunctions
reviewed: 2025-03-14
isUpgradeGuide: true
---

## Default topology has changed

This version of the functions integration with Azure Service Bus uses NServiceBus.Transport.AzureServiceBus version 5 and higher. By default Azure Service Bus is configured to the [default topology](/transports/azure-service-bus/topology.md) (using topic-per-event approach). The defaults can be overridden by adding topology options to the Application configuration

The functions integration looks for either topology options placed into `AzureServiceBus:TopologyOptions` or `AzureServiceBus:MigrationTopologyOptions` for migration and backward compatibility scenarios.

snippet: asb-function-topology-options

Using the default topology

snippet: asb-function-topology-options-json

Using the migration topology

snippet: asb-function-topology-migration-options-json