---
title: Custom audit transport
summary: Azure Service Bus endpoint with audit via Azure Storage Queue
reviewed: 2025-05-28
component: Core
related:
 - transports/azure-service-bus
 - transports/azure-storage-queues
 - nservicebus/pipeline/features
 - nservicebus/pipeline/manipulate-with-behaviors
---

This sample demonstrates how an endpoint can use a different transport for audting to its main transport.
In this instance, Azure Service Bus is the main transport used by the endpoint, and Azure Storage Queues is used as the transport for audit messages.

> [!WARNING]
> Two different transports are being used, which means the Azure Storage Queue transport dispatcher does not participate in the handler transaction. Hence there could be a scenario where the audit message is successfully sent, but an error occurs later in the pipeline that causes the receiver operation to be rolled back. This would result in two conflicting instances of the same message being visible in the Service Pulse [all messages view](/servicepulse/all-messages.md), one showing as an error and the other as successfully processed.

## Prerequisites

Ensure that:

- an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage) is running
- the Azure Service Bus connection is stored in an environment variable called `AzureServiceBus_ConnectionString`
- the `audit` queue exists in Azure Storage Queues

### Optional

To see how the auditing messages appear in ServicePulse, two ServiceControl instances will need to be setup.

- [ServiceControl Error instance](/servicecontrol/servicecontrol-instances/) with an Azure Service Bus transport and the same connection string as used in the sample.
- [ServiceControl Audit instance](/servicecontrol/audit-instances/) with an Azure Storage Queue transport and the same connection string as used in the sample (the default local emulator `UseDevelopmentStorage=true`).

## Projects

### CustomAuditTransport

Main endpoint that is running on Azure Service Bus. It enables the `audit` feature.

### AuditViaASQ

A [feature](/nservicebus/pipeline/features.md) that uses Azure Storage Queues for audit messages instead of the transport used by the endpoint being audited.

The feature is turned on by default providing that Auditing is enabled and an audit queue has been defined.

snippet: featureSetup

It uses a PipelineTerminator, which is a special type of [behavior](/nservicebus/pipeline/manipulate-with-behaviors.md), to replace the existing [AuditToRoutingConnector](https://github.com/Particular/NServiceBus/blob/master/src/NServiceBus.Core/Audit/AuditToRoutingConnector.cs) process, which is the last step in the audit pipeline.

snippet: auditToDispatchConnectorReplacement

snippet: auditDispatchTerminator

## Running the sample

Build the solution and run the `CustomAuditTransport` project.

Press `s` to send a message that will be successfully processed.
Press `e` to send a message that will error.

Open ServicePulse and see how the messages appear in the `All Messages` view.
