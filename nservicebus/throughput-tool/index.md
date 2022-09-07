---
title: Measuring system throughput
summary: Use the Particular throughput tool to measure the throughput of your NServiceBus system.
reviewed: 2022-08-30
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time, which can then be extrapolated to daily throughput numbers.

The tool currently supports collecting data from [ServiceControl performance metrics](/monitoring/metrics/install-plugin.md) normally [displayed in ServicePulse](/monitoring/metrics/in-servicepulse.md), or by directly interrogating the message transport when using the [Azure Service Bus](/transports/azure-service-bus/), [RabbitMQ](/transports/rabbitmq/), or [SQL Sever](/transports/sql/) transports.

The ServiceControl collection method is preferred and also can be run instantly rather than requiring 15 minutes to complete.

## Installation

To install the tool:

1. Install either [.NET Core 3.1 or .NET 6.0](https://dotnet.microsoft.com/en-us/download).
2. From a terminal window, use the following command to install the Particular.EndpointThroughputCounter from MyGet:
    ```shell
    dotnet tool install -g Particular.EndpointThroughputCounter --add-source=https://www.myget.org/F/particular/api/v3/index.json
    ```

### Updating

To update the tool to the latest version, execute the following command in a terminal window:

```shell
dotnet tool update -g Particular.ThroughputTool --add-source https://www.myget.org/F/particular/api/v3/index.json
```

### Uninstalling

To uninstall the tool, execute the following command in a terminal window:

```shell
dotnet tool uninstall -g Particular.EndpointThroughputCounter
```

## Usage

First, determine which method of data collection to use:

* **(Preferred) [ServiceControl data collection](#usage-servicecontrol)**: Use when the system employs [monitoring performance metrics in ServicePulse](/monitoring/metrics/in-servicepulse.md) and all endpoints have the [monitoring plug-in](/monitoring/metrics/install-plugin.md) installed.
* **[Azure Service Bus](#usage-azure-service-bus):**: Use for Azure Service Bus systems.
* **[RabbitMQ](#usage-rabbitmq)**: Use for RabbitMQ systems when ServiceControl is unavailable.
* **[SQL Transport](#usage-sql-transport)**: Use for SQL transport systems when ServiceControl is unavailable.

### ServiceControl

Once installed, execute the tool with the URLs for the ServiceControl and monitoring APIs, as in this example:

```shell
throughput-counter servicecontrol --serviceControlApiUrl http://localhost:33333/api/ --monitoringApiUrl http://localhost:33633/ --outputPath throughput-report.json
```

Because ServiceControl contains, at maximum, the previous 1 hour of monitoring data, the tool will query the ServiceControl API 24 times with a one-hour sleep period between each attempt in order to capture a total of 24 hours worth of data.

#### Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--serviceControlApiUrl`</nobr> | The URL of the ServiceControl API. In the [ServiceControl Management Utility](/servicecontrol/installation.md), find the instance identified as a **ServiceControl Instance** and use the value of the **URL** field, as shown in the screenshot below. |
| <nobr>`--monitoringApiUrl`</nobr> | The URL of the Monitoring API. In the [ServiceControl Management Utility](/servicecontrol/installation.md), find the instance identified as a **Monitoring Instance** and use the value of the **URL** field, as shown in the screenshot below. |
| <nobr>`--outputPath`</nobr> | A local path where the JSON report file should be generated. |

This screenshot shows how to identify the instance types and locate the required URLs:

![ServiceControl instances showing tool inputs](servicecontrol.png)

### Azure Service Bus

Collecting metrics from Azure Service Bus relies upon the Azure Command Line Interface (CLI), which must be installed first.

1. Install a version of [Powershell or Powershell Core](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell) on the host system.
1. Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).
1. From a command line, execute `az login`, which will open a browser to complete the authentication to Azure. The Azure login must have access to view metrics data for the Azure Service Bus namespace.
1. Execute `az set --subscription {SubscriptionId}`, where `SubscriptionId` is a Guid matching the subscription id that contains the Azure Service Bus namespace.
1. In the Azure Portal, go to the Azure Service Bus namespace, click **Properties** in the side navigtation (as shown in the screenshot below) and then copy the `Id` value, which will be needed to run the tool. The `Id` value should have a format similar to `/subscriptions/{Guid}/resourceGroups/{rsrcGroupName}/providers/Microsoft.ServiceBus/namespaces/{namespaceName}`.

This screenshot shows how to copy the `SubscriptionId` value:

![How to collect the Subscription Id](azure-service-bus.png)

Once these prerequisites are complete, execute the tool with the resource id of the Azure Service Bus namespace, as in this example:

```shell
throughput-counter azureservicebus --resourceId /subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/resourceGroups/my-resource-group/providers/Microsoft.ServiceBus/namespaces/my-asb-namespace --outputPath throughput-report.json
```

The tool may open additional terminal windows, which are Powershell processes gathering the data from the Azure CLI.

Unlike ServiceControl, using Azure Service Bus metrics allows the tool to capture the last 30 days worth of data at once, which means that the report will be generated without delay.

#### Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--resourceId`</nobr> | The resource ID of the Azure Service Bus namespace, which can be found in the Azure Portal as described above. |
| <nobr>`--outputPath`</nobr> | A local path where the JSON report file should be generated. |

### RabbitMQ

To collect data from RabbitMQ, the [management plugin](https://www.rabbitmq.com/management.html) must be enabled on the RabbitMQ broker. The tool will also require a login that can access the management UI.

Execute the tool with the RabbitMQ management URL, as in this example where the RabbitMQ broker is running on localhost:

```shell
throughput-counter rabbitmq --apiUrl http://localhost:15672 --outputPath throughput-report.json
```

The tool will prompt for the username and password to access the RabbitMQ management interface. After that, it will take its initial reading, then sleep for 24 hours before taking its final reading and generating a report.

#### Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--apiUrl`</nobr> | The URL for the RabbitMQ management site. |
| <nobr>`--outputPath`</nobr> | A local path where the JSON report file should be generated. |

### SQL Transport

Once installed, execute the tool with the database connection string used by SQL Server endpoints, as in this example:

```shell
throughput-counter sqlserver --connectionString "Server=SERVER;Database=DATABASE;User=USERNAME;Password=PASSWORD;" --outputPath throughput-report.json
```

The tool will run for slightly longer than 24 hours in order to capture a beginning and ending `RowVersion` value for each queue table. A value can only be detected when a message is waiting in the queue to be processed, and not from an empty queue, so multiple queries may be required. The tool will use a backoff mechanism to avoid putting undue pressure on the SQL Server instance.

#### Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--connectionString`</nobr> | The database connection string that will provide at least read access to all queue tables. |
| <nobr>`--outputPath`</nobr> | A local path where the JSON report file should be generated. |

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