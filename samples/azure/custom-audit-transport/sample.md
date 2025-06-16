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

This sample demonstrates how an endpoint can audit to a different transport than its main transport.
In this instance, Azure Service Bus is the main transport used by the endpoint, and Azure Storage Queues is used as the transport for audit messages.

> [!WARNING]
> Two different transports are being used, which means the Azure Storage Queue transport dispatcher does not participate in the handler transaction. This could result in a scenario where the audit message is successfully sent, but an error occurs later in the pipeline that causes the receiver operation to be rolled back. The result of this would be two conflicting instances of the same message being visible in the ServicePulse [all messages view](/servicepulse/all-messages.md), one showing as an error and the other (erroneously) as successfully processed.
>
> Additionally, the audit instance cannot directly communicate with the error instance, hence plugin messages (e.g. Heartbeats, Custom Checks, Saga Audit, etc.) will fail to send. There will be [No destination specified for message](/servicecontrol/troubleshooting.md#no-destination-specified-for-message) errors in the audit instance log file if these features are used.

## Prerequisites

Ensure that:

- an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage) is running
- the Azure Service Bus connection is stored in an environment variable called `AzureServiceBus_ConnectionString`
- the `audit` queue exists in Azure Storage Queues

### Optional

To see how the auditing messages appear in ServicePulse, two ServiceControl instances will need to be setup.

- [ServiceControl Error instance](/servicecontrol/servicecontrol-instances/) with an Azure Service Bus transport and the same connection string as used in the sample.
- [ServiceControl Audit instance](/servicecontrol/audit-instances/) with an Azure Storage Queue transport and the same connection string as used in the sample (the default local emulator `UseDevelopmentStorage=true`).

A `docker-compose.yml` file is provided which will [setup the instances in a local container environment](#running-the-sample-servicecontrol-and-servicepulse).

## Projects

### CustomAuditTransport

The main endpoint that is running on Azure Service Bus. It enables the `audit` feature.

The configured audit queue name, specified in `AuditProcessedMessagesTo`, will be used as the queue name on the Azure Storage Queue transport.

### AuditViaASQ

A [feature](/nservicebus/pipeline/features.md) that uses Azure Storage Queues for audit messages instead of the transport used by the endpoint being audited.

The feature is turned on by default if Auditing is enabled and an audit queue has been defined.

snippet: featureSetup

It uses a PipelineTerminator, which is a special type of [behavior](/nservicebus/pipeline/manipulate-with-behaviors.md). This replaces the existing [AuditToRoutingConnector](https://github.com/Particular/NServiceBus/blob/master/src/NServiceBus.Core/Audit/AuditToRoutingConnector.cs) process, which is the last step in the audit pipeline.

snippet: auditToDispatchConnectorReplacement

snippet: auditDispatchTerminator

## Running the sample

### CustomAuditTransport endpoint

Build the solution and run the `CustomAuditTransport` project.

Press `s` to send a message that will be successfully processed.
Press `e` to send a message that will error.

### ServiceControl and ServicePulse

#### Pull the latest images

Before running the containers, ensure you're using the latest version of each image by executing the following command:

 ```shell
 docker compose pull
 ```

This command checks for any updates to the images specified in the docker-compose.yml file and pulls them if available.

#### Start the containers

After pulling the latest images, modify the `service-platform-error.env` and `env/service-platform-audit.env` environment files, if necessary, and then start up the containers using:

```shell
docker compose up -d
```

Once composed, [ServicePulse](/servicepulse/) can be accessed at [http://localhost:9090](http://localhost:9090) to see how the messages appear in the `All Messages` view.

#### Implementation details

- The ports for all services are exposed to localhost:
  - `33333`: ServiceControl API
  - `44444`: Audit API
  - `8080`: Database backend
  - `9090` ServicePulse UI
- One instance of the [`servicecontrol-ravendb` container](/servicecontrol/ravendb/containers.md) is used for both the [`servicecontrol`](/servicecontrol/servicecontrol-instances/deployment/containers.md) and [`servicecontrol-audit`](/servicecontrol/audit-instances/deployment/containers.md) containers.
  - _A single database container should not be shared between multiple ServiceControl instances in production scenarios._
