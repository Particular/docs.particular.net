---
title: Azure Service Bus CloudEvents Sample
summary: Demonstrates how to consume CloudEvents received through Azure Service Bus
reviewed: 2025-12-08
component: ASBS
related:
- transports/azure-service-bus
---

## Prerequisites

include: asb-connectionstring-xplat

Azure Event Grid needs to be configured to receive Azure Blob Storage notifications and deliver them to Azure Service Bus using the CloudEvents schema. The configuration is described in the [Microsoft documentation](https://learn.microsoft.com/en-us/azure/event-grid/cloud-event-schema#sample-event-using-cloudevents-schema).

## Code walk-through

This sample shows an endpoint receiving a CloudEvents message from Azure Service Bus. Such a message can be created by Azure Blob Storage and delivered to Azure Service Bus via Azure Event Grid.

* The `Endpoint` uses the Azure EventGrid SDK to access the schema of the CloudEvents message.
* The `Endpoint` enables CloudEvents support and configures the type mapping.
* The `Endpoint` configures the serializer to support fields and properties with different casing.
* The `Endpoint` receives the message and calls the proper handler.

### CloudEvents message schema

The message schema is defined in the [Azure Event Grid SDK](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/messaging.eventgrid-readme?view=azure-dotnet).

This schema matches the [Event Grid notifications schema](https://learn.microsoft.com/en-us/azure/event-grid/event-schema-blob-storage?tabs=cloud-event-schema#microsoftstorageblobcreated-event).

### CloudEvents support configuration

CloudEvents support must be explicitly enabled:

snippet: asb-cloudevents-configuration

The configuration includes the type mapping to match the messages with the classes:

snippet: asb-cloudevents-typemapping

To support the differences between uppercase letters and lowercase letters in the schema definition and content, the serializer is configured to use case insensitive mapping:

snippet: asb-cloudevents-serialization

### Running the sample

1. Run the sample.
2. Generate the `BlobCreated` event by creating a new file in the Azure Blob Storage container.
3. Observe that the sample prints out the URL of the newly created file.
