---
title: Measuring system throughput
summary: Use the Particular throughput tool to measure the throughput of your NServiceBus system.
reviewed: 2022-08-30
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time, which can then be extrapolated to daily throughput numbers.

The tool currently supports collecting data from [ServiceControl performance metrics](/monitoring/metrics/install-plugin.md) normally [displayed in ServicePulse](/monitoring/metrics/in-servicepulse.md), or by directly interrogating the message transport when using the [RabbitMQ](/transports/rabbitmq/) or [SQL Sever](/transports/sql/) transports.

The ServiceControl collection method is preferred and also can be run instantly rather than requiring 15 minutes to complete.

## Installation

To install the tool:

1. Install [.NET 6.0](https://dotnet.microsoft.com/en-us/download).
2. From a terminal window, use the following command to install the Particular.ThroughputTool from MyGet:
    ```shell
    dotnet tool install -g Particular.ThroughputTool --add-source=https://www.myget.org/F/particular/api/v3/index.json
    ```

### Updating

To update the tool to the latest version, execute the following command in a terminal window:

```shell
dotnet tool update -g Particular.ThroughputTool --add-source https://www.myget.org/F/particular/api/v3/index.json
```

## Usage

First, determine which method of data collection to use:

* **(Preferred) [ServiceControl data collection](#usage-servicecontrol)**: Use when the system employs [monitoring performance metrics in ServicePulse](/monitoring/metrics/in-servicepulse.md) and all endpoints have the [monitoring plug-in](/monitoring/metrics/install-plugin.md) installed.
* **[RabbitMQ](#usage-rabbitmq)**: Use for RabbitMQ systems when ServiceControl is unavailable.
* **[SQL Transport](#usage-sql-transport)**: Use for SQL transport systems when ServiceControl is unavailable.

### ServiceControl

Once installed, execute the tool with the URLs for the ServiceControl and monitoring APIs:

```shell
particular-throughput-tool servicecontrol    http://localhost:33633/ --outputPath report.json
```

#### Options

All options are required:

| Option | Description |
|-|-|
| <nobr>`--serviceControlApiUrl`</nobr> | The URL of the ServiceControl API. In the ServiceControl Management Utility, find the instance identified as a **ServiceControl Instance** and use the value of the **URL** field. |
| <nobr>`--monitoringApiUrl`</nobr> | The URL of the Monitoring API. In the ServiceControl Management Utility, find the instance identified as a **Monitoring Instance** and use the value of the **URL** field. |
| <nobr>`--outputPath`</nobr> | A local path where the JSON report file should be generated. |

#### Example

```shell
particular-throughput-tool servicecontrol --serviceControlApiUrl http://localhost:33333/api/ --monitoringApiUrl http://localhost:33633/ --outputPath throughput-report.json
```

### RabbitMQ

### SQL Transport

