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

Kafka is an event streaming broker, similar to Azure Event Hubs. Event streaming brokers are used for partitioned logs, to store massive amounts of events that are coming in, for example from IoT devices. NServiceBus on the other hand works on top of messaging brokers like Azure Service Bus, RabbitMQ and Amazon SQS/SNS. They can be used to complement each other as shown in this sample, which has two projects.

- A ConsoleEndpoint is the starting point of the sample, which products numerous events to Kafka.
- An Azure Function using a Kafka trigger to consume the events and at a certain point sends a message using an NServiceBus SendOnly endpoint back to the ConsoleEndpoint.

For more information about Kafka and NServiceBus read our blogpost [Let's talk about Kafka](https://particular.net/blog/lets-talk-about-kafka).

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

1. The console window for `ConsoleEndpoint` accepts <key>ENTER</key> to start producing events into Kafka.
1. The Azure Function will consume the events and verify if the event contains information that indicates a certain threshold has been reached. In that case it will send a `FollowUp` message with NServiceBus.
1. The console window `ConsoleEndpoint` will receive the `FollowUp` message and process it with NServiceBus.

## Code walk-through

The NServiceBus endpoint configured using `IFunctionHostBuilder` in the `Startup` class like this:

snippet: ProduceEvent

Note the `NServiceBusTriggerFunction` is used to automatically generate the Azure Functions trigger code that is needed to invoke NServiceBus.

### Handlers

These are the message handlers, with a `CustomDependency` passed in.

snippet: SetupNServiceBusSendOnly
