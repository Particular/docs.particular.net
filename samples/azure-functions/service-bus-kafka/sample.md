---
title: Using NServiceBus and Kafka in Azure Functions (isolated)
summary: Using NServiceBus with Kafka triggers and Azure Functions isolated worker hosting model.
component: ASBFunctionsWorker
related:
 - nservicebus/hosting/azure-functions-service-bus
redirects:
 - samples/previews/azure-functions/kafka
reviewed: 2023-09-13
---

This sample shows how to process Kafka events using an Azure Functions trigger and how to follow up on those events using an NServiceBus SendOnly endpoint in an Azure Function.

### Kafka and NServiceBus

[Kafka](https://kafka.apache.org/) is an event streaming broker, similar to [Azure Event Hubs](https://azure.microsoft.com/en-us/products/event-hubs). Event streaming brokers are used to store massive amounts of events that are coming in, for example from IoT devices. NServiceBus on the other hand works on top of messaging brokers like Azure Service Bus, RabbitMQ and Amazon SQS/SNS. They can complement each other as shown in this sample, which has two projects.

- A ConsoleEndpoint is the starting point of the sample, which products numerous events to Kafka.
- An Azure Function using a Kafka trigger to consume the events and at a certain point sends a message using an NServiceBus SendOnly endpoint back to the ConsoleEndpoint.

For more information about Kafka and NServiceBus read the blogpost [Let's talk about Kafka](https://particular.net/blog/lets-talk-about-kafka).

downloadbutton

## Prerequisites

### Configure Connection string

To use the sample a valid Service Bus connection string must be provided

- in the `local.settings.json` file in the `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` project.

- as an environment variable named `AzureServiceBus_ConnectionString`.

### Kafka broker

This sample requires Kafka available to store the events being produced.

#### Set up Kafka Docker container

To set up a Docker container with Kafka, the sample contains a `docker-compose.yml` file. From a CLI like PowerShell, using the following command in the solution folder, a Docker image can be downloaded and executed:

```
docker-compose up
```

## Sample structure

The sample contains the following projects:
- `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` - Azure function with Kafka trigger and NServiceBus SendOnly endpoint
- `AzureFunctions.Messages` - message definitions
- `ConsoleEndpoint` - NServiceBus endpoint and Kafka producer

## Running the sample

The solution requires both `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` and `ConsoleEndpoint` to run.

After the sample is running with both projects:

1. The console window for `ConsoleEndpoint` accepts <kbd>ENTER</kbd> to start producing events into Kafka.
1. The Azure Function will consume the events and verify if the event contains information that indicates a certain threshold has been reached. In that case it will send a `FollowUp` message with NServiceBus.
1. The console window `ConsoleEndpoint` will receive the `FollowUp` message and process it with NServiceBus.

## Code walk-through

The project `ConsoleEndpoint` produces the events as follows:

snippet: ProduceEvent

Kafka events can only contains strings for values. The Kafka SDK for .NET supports using schemas and serialization of events, but for simplicity reasons the event `ElectricityUsage` is serialized using Newtonsoft.Json.

### Kafka trigger

A Kafka trigger in project `AzureFunctions.KafkaTrigger.FunctionsHostBuilder` consumes the event and verifies if the electricity usage for any customers and any of its units goes over a certain threshold. If so an NServiceBus SendOnly endpoint is used to send the message. The following code shows how to set up an NServiceBus endpoint in Azure Functions and register the `IMessageSession` instance with the dependency injection container:

snippet: SetupNServiceBusSendOnly

In the Kafka trigger, again for simplicity reasons, it is verified if the event has the value of `42` and a message is send back to the `ConsoleEndpoint`.

snippet: KafkaTrigger

The `FollowUp` message is then received in the `ConsoleEndpoint` in the NServiceBus `FollowUpHandler`  message handler.

