---
title: Measuring system throughput
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

## Installation

The tool can be installed as a .NET tool for Windows/Linux or as a self-contained Windows executable.

### .NET tool install (preferred)

1. Install [.NET 6.0](https://dotnet.microsoft.com/en-us/download).
2. From a terminal window, use the following command to install the throughput counter from MyGet:
    ```shell
    dotnet tool install -g Particular.EndpointThroughputCounter --add-source=https://www.myget.org/F/particular/api/v3/index.json
    ```
3. Run the tool by executing `throughput-counter`:
    ```shell
    throughput-counter <arguments>
    ```

### Self-contained executable

In this mode, the target system does not need any version of .NET preinstalled:

1. [Download the self-contained Windows executable](https://s3.amazonaws.com/particular.downloads/EndpointThroughputCounter/Particular.EndpointThroughputCounter.zip)
2. Unzip the downloaded file.
3. Execute the tool using its full name from the folder in which it was downloaded:
    ```shell
    Particular.EndpointThroughputCounter.exe <arguments>
    ```

## Running the tool

The tool can collect data using a variety of methods depending upon the system's configuration. To run the tool, select the relevant article based on the [message transport](/transports/) used in the system to be measured:

* [Azure Service Bus](azure-service-bus.md)
* [Amazon SQS](amazon-sqs.md)
* [RabbitMQ](rabbitmq.md)
* [SQL Transport](sql-transport.md)
* Microsoft Message Queueing (MSMQ) – Use [ServiceControl data collection](service-control.md)
* Azure Storage Queues – Use [ServiceControl data collection](service-control.md)

## Masking private data

The report that is generated will contain the names of endpoints/queues. If the queue names themselves contain confidential or proprietary information, certain strings can be masked in the report file.

```shell
throughput-counter <command> <options> --queueNameMasks Samples
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
dotnet tool update -g Particular.EndpointThroughputCounter --add-source https://www.myget.org/F/particular/api/v3/index.json
```

## Uninstalling

To uninstall the tool, execute the following command in a terminal window:

```shell
dotnet tool uninstall -g Particular.EndpointThroughputCounter
```

## Questions

Check out the [frequently asked questions](faq.md).