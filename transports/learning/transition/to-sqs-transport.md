---
title: Transitioning to the Amazon SQS Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the Amazon SQS Transport
reviewed: 2019-04-24
component: SQS
tags:
 - AWS
 - Transport
related:
 - transports/sqs
 - samples/sqs/simple
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning Transport to the Amazon SQS Transport.


## Install the NuGet Package

The [NServiceBus.AmazonSQS](https://www.nuget.org/packages/NServiceBus.AmazonSQS/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.AmazonSQS
```

## Change the Endpoint Configuration

To use the Amazon SQS Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<SqsTransport>();
```


### Enable Installers

The endpoint must enable installers to allow the Amazon SQS Transport to create the necessary [queues](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-basic-architecture.html) for your endpoint.

snippet: Installers


### Configure a Persistence

This is because the Amazon SQS Transport, unlike the Learning Transport, does not natively support Publish/Subscribe and instead uses [message-driven Pub/Sub](/nservicebus/messaging/publish-subscribe.md#mechanics-message-driven-persistence-based), so the message subscription information must be stored.

snippet: SqlPersistenceUsageMySql

[SQL Persistence using MySQL Server](/persistence/sql/) is shown here as an example, however use the [selecting a persister](/persistence/selecting.md) guide to help determine the appropriate persistence for the endpoint.


include: registerpublishers
