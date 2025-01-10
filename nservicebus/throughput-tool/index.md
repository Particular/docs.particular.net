---
title: Endpoint Throughput tool
summary: Use the Particular endpoint throughput counter tool to measure the usage of an NServiceBus system.
reviewed: 2024-05-22
related:
  - nservicebus/usage
---

> [!NOTE]
> The recommended method of measuring the throughput of an NServiceBus system is via [ServicePulse](./../../servicepulse/usage.md).
> The endpoint throughput counter tool is offered as an alternate option for customers that aren't able to use ServicePulse.

The Particular endpoint throughput counter tool can typically be installed on a [user's workstation](faq.md#does-the-tool-need-to-run-on-my-production-server) and run against a production system to measure the throughput of each endpoint over a period of time.

## Installation

The tool can be installed as a .NET tool for Windows/Linux or as a self-contained Windows executable.

### .NET tool (recommended)

1. Install [.NET 8.0](https://dotnet.microsoft.com/en-us/download).
1. From a terminal window, use the following command to install the throughput counter:

   ```shell
   dotnet tool install -g Particular.EndpointThroughputCounter --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
   ```

1. Run the tool by executing `throughput-counter`:

   ```shell
   throughput-counter [command] [options]
   ```

### Self-contained executable

In this mode, the target system does not need any version of .NET preinstalled.

1. Download the [self-contained Windows executable](https://s3.amazonaws.com/particular.downloads/EndpointThroughputCounter/Particular.EndpointThroughputCounter.zip).
1. Unzip the downloaded file.
1. Open a terminal window and navigate to the folder to which it was downloaded.
1. Execute the tool from the terminal by using its full name:

   ```shell
   Particular.EndpointThroughputCounter.exe [command] [options]
   ```

## Running the tool

The tool can collect data using various methods depending on the system's configuration. To run the tool, select the relevant article based on the [message transport](/transports/) used in the system to be measured:

- [Azure Service Bus](azure-service-bus.md)
- [Amazon SQS](amazon-sqs.md)
- [RabbitMQ](rabbitmq.md)
- [SQL Transport](sql-transport.md)
- [PostgreSQL Transport](postgresql-transport.md)
- Microsoft Message Queueing (MSMQ) – Use [ServiceControl data collection](service-control.md)
- Azure Storage Queues – Use [ServiceControl data collection](service-control.md)
- [Click here if unsure what message transport is used by the system](determine-transport.md)

> [!TIP]
> If the system uses MSMQ or Azure Storage Queues but does not use ServiceControl, this tool cannot be used to measure throughput.
>
> If this or any other problem is encountered attempting to generate a throughput report, [open a support case](https://customers.particular.net/request-support/licensing).

## Masking private data

The generated report will contain the names of endpoints/queues. Certain strings can be masked in the report file if the queue names contain confidential or proprietary information.

```shell
throughput-counter [command] [options] --queueNameMasks Samples
```

This will result in a report file with masked data, such as:

```json
{
  "QueueName": "***.RabbitMQ.SimpleReceiver",
  "Throughput": 0
}
```

## Updating

To update the tool to the latest version, execute the following command in a terminal window:

```shell
dotnet tool update -g Particular.EndpointThroughputCounter --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
```

## Uninstalling

To uninstall the tool, execute the following command in a terminal window:

```shell
dotnet tool uninstall -g Particular.EndpointThroughputCounter
```

## Questions

Check out [frequently asked questions (FAQ)](faq.md) about the endpoint throughput counter tool.
