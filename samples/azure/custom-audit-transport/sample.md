---
title: Custom audit transport
summary: Using ASQ for auditing while using ASB as the main transport
reviewed: 2025-05-28
component: Core
---

This sample shows how an endpoint can be using one transport, and utlising a different transport for auditing.
In this instance, Azure Service Bus is the main transport used by the endpoint, and Azure Storage Queues is used as the transport for audit messages.

## Prerequisites

Ensure that:

- an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage) is running
- the Azure Service Bus connection is stored in an environment variable called `AzureServiceBus_ConnectionString`
- the `audit` queue exist in Azure Storage Queues

## Projects

### CustomAuditTransport

Main endpoint that is running on Azure Service Bus. It enables the `audit` feature.

### AuditViaASQ

Feature that uses Azure Storage Queues for audit messages instead of the transport used by the endpoint being audited.

The feature is turned on by default providing that Auditing is enabled and an audit queue has been defined.

snippet: featureSetup

It uses [behavior](/nservicebus/pipeline/manipulate-with-behaviors) to register a new step to run after the inbuilt steps of the audit pipeline.

snippet: behaviorRegistration

## Running the sample








