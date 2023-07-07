---
title: Measuring system throughput
summary: Use the Particular endpoint throughput counter tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
---

The Particular endpoint throughput counter tool can be installed locally and run against a production system to measure the throughput of each endpoint over a period of time.

## Installation

The tool can be installed as a .NET tool for Windows/Linux or as a self-contained Windows executable.

### .NET tool (recommended)

1. Install [.NET 6.0](https://dotnet.microsoft.com/en-us/download).
1. From a terminal window, use the following command to install the throughput counter from MyGet:

    ```shell
    dotnet tool install -g Particular.EndpointThroughputCounter --add-source=https://www.myget.org/F/particular/api/v3/index.json
    ```

1. Run the tool by executing `throughput-counter`:

    ```shell
    throughput-counter [command] [options]
    ```

### Self-contained executable

In this mode, the target system does not need any version of .NET preinstalled.

1. Download the [self-contained Windows executable](https://s3.amazonaws.com/particular.downloads/EndpointThroughputCounter/Particular.EndpointThroughputCounter.zip).
1. Unzip the downloaded file.
1. Open a terminal window and navigate to folder to which it was downloaded.
1. Execute the tool from the terminal by using its full name:

    ```shell
    Particular.EndpointThroughputCounter.exe [command] [options]
    ```

### Containers

If the system is running in containers, the throughput tool can be run from a container that is based on the [Microsoft .NET SDK `mcr.microsoft.com/dotnet/sdk6.0` image](https://hub.docker.com/_/microsoft-dotnet-sdk/). Some systems will have such a container provisioned to access for system administration tasks.

1. Launch a container based on `mcr.microsoft.com/dotnet/sdk6.0` and open a remote bash to this container instance:
    ```shell
    // For example, use Docker and launch a bash shell
    docker run -it --name=throughputtool mcr.microsoft.com/dotnet/sdk:6.0 bash
    ```
1. Use the following command to install the throughput counter from MyGet:
    ```shell
    dotnet tool install -g Particular.EndpointThroughputCounter --add-source=https://www.myget.org/F/particular/api/v3/index.json
    ```
1. Run the tool by executing `throughput-counter`:
    ```shell
    throughput-counter [command] [options]
    ```
1. When the tool completes, find the JSON report file output by the tool:
    ```shell
    ls *.json
    ```
1. Transfer the file to your local system so it can be sent to Particular Software.
    ```shell
    # Example: Use cat to write the report to the console. This might not be ideal when dealing with large reports.
    cat ./customer-name-YYYYMMDD-HHMMSS.json

    # Example: Upload the via to an intermediary like. For example, use https://transfer.sh to upload the report, navigate to the returned url, download the report, and delete the file from the service.
    curl --upload-file ./customer-name-YYYYMMDD-HHMMSS.json https://transfer.sh
    ```

## Running the tool

The tool can collect data using a variety of methods depending upon the system's configuration. To run the tool, select the relevant article based on the [message transport](/transports/) used in the system to be measured:

* [Azure Service Bus](azure-service-bus.md)
* [Amazon SQS](amazon-sqs.md)
* [RabbitMQ](rabbitmq.md)
* [SQL Transport](sql-transport.md)
* Microsoft Message Queueing (MSMQ) – Use [ServiceControl data collection](service-control.md)
* Azure Storage Queues – Use [ServiceControl data collection](service-control.md)
* [Click here if unsure what message transport is used by the system](determine-transport.md)

If the system uses MSMQ or Azure Storage Queues but does not use ServiceControl, this tool cannot be used to measure throughput. Email <a href="mailto:contact@particular.net">contact@particular.net</a> for instructions on estimating system throughput.

## Masking private data

The report that is generated will contain the names of endpoints/queues. If the queue names themselves contain confidential or proprietary information, certain strings can be masked in the report file.

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
dotnet tool update -g Particular.EndpointThroughputCounter --add-source https://www.myget.org/F/particular/api/v3/index.json
```

## Uninstalling

To uninstall the tool, execute the following command in a terminal window:

```shell
dotnet tool uninstall -g Particular.EndpointThroughputCounter
```

## Questions

Check out [frequently asked questions (FAQ)](faq.md) about the endpoint throughput counter tool.
