---
title: Using NServiceBus and Kafka in Azure Functions (isolated)
summary: Using NServiceBus with Kafka triggers and Azure Functions isolated worker hosting model.
component: ASBS
related:
 - nservicebus/hosting/azure-functions-service-bus
redirects:
 - samples/previews/azure-functions/kafka
reviewed: 2025-08-12
---

This sample shows how to process Kafka events using an Azure Functions trigger and how to follow up on those events using an NServiceBus SendOnly endpoint in an Azure Function.

### Kafka and NServiceBus

[Kafka](https://kafka.apache.org/) is an event streaming broker, similar to [Azure Event Hubs](https://azure.microsoft.com/en-us/products/event-hubs). Event streaming brokers are used to store massive amounts of incoming events, for example from IoT devices. In contrast, NServiceBus works on top of messaging brokers like Azure Service Bus, RabbitMQ, and Amazon SQS/SNS. They can complement each other as shown in this sample consisting of two projects.

- The ConsoleEndpoint is the starting point of the sample, which produces numerous Kafka events.
- The Azure Function uses a Kafka trigger to consume the events and send NServiceBus messages back to the ConsoleEndpoint via Azure ServiceBus.

For more information about Kafka and NServiceBus, read the blogpost [Let's talk about Kafka](https://particular.net/blog/lets-talk-about-kafka).

downloadbutton

## Prerequisites

### Service bus connection string

To use the sample, provide a valid Azure Service Bus connection string from a namespace that supports topics and sessions (Standard or Premium tier). Add it to the `local.settings.json file` in the `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` project and set it as an environment variable named `AzureServiceBus_ConnectionString`.

### Kafka broker

This sample requires Kafka to store the produced events.

#### Setting up a Kafka Docker container

The sample contains a `docker-compose.yml` file to set up a Docker container with Kafka. From a CLI like PowerShell, use the following command in the solution folder to download and execute a Docker image:

```
docker-compose up
```

> [!NOTE]
> When running Docker in Windows, it's possible to get an error saying _no matching manifest for windows/amd64 in the manifest list entries_. This can be solved by running the daemon in [experimental mode](https://stackoverflow.com/questions/48066994/docker-no-matching-manifest-for-windows-amd64-in-the-manifest-list-entries).

## Sample structure

The sample contains the following projects:
- `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` - Azure function with Kafka trigger and NServiceBus SendOnly endpoint
- `AzureFunctions.Messages` - message definitions
- `ConsoleEndpoint` - NServiceBus endpoint and Kafka producer

## Running the sample

The solution requires both `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` and `ConsoleEndpoint` to run.

After the sample is running with both projects:

1. Press <kbd>ENTER</kbd> in the `ConsoleEndpoint` window to start producing Kafka events.
1. The Azure Function will consume each Kafka event and check if it contains information indicating a specific threshold has been reached. When it has, the function will send a `FollowUp` Azure Service Bus message to the ConsoleEndpoint using NServiceBus.
1. The console window `ConsoleEndpoint` will receive the `FollowUp` message and process it with NServiceBus.

## Code walk-through

The project `ConsoleEndpoint` produces the events as follows:

snippet: ProduceEvent

Kafka events can only contain strings for values. The Kafka SDK for .NET supports using schemas and serialization of events, but for simplicity, the `ElectricityUsage` event is serialized using Newtonsoft.Json.

### Kafka trigger

A Kafka trigger in project `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` consumes the event and checks if the electricity usage for any customers and any of a customer's units goes over a specified threshold. If so, an NServiceBus SendOnly endpoint is used to send a message. The following code shows how to set up an NServiceBus endpoint in Azure Functions and register the `IMessageSession` instance with the dependency injection container:

snippet: SetupNServiceBusSendOnly

In the Kafka trigger, it checks for an event value of `42` (hard-coded for simplicity sake), in which case, a message is sent back to the `ConsoleEndpoint`.

snippet: KafkaTrigger

The `FollowUp` message is then received in the `ConsoleEndpoint` in the NServiceBus `FollowUpHandler`  message handler.

