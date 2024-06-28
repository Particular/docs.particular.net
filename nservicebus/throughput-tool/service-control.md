---
title: Measuring system throughput with ServiceControl data
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
related:
  - nservicebus/throughput-tool
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

This article details how to collect endpoint and throughput data using data from [ServiceControl](/servicecontrol/). Refer to the [throughput counter main page](./) for information how to install/uninstall the tool or for other data collection options.

The tool should be used with a [supported version of ServiceControl](/servicecontrol/upgrades/supported-versions.md).

> [!NOTE]
> Do not attempt to install ServiceControl just to run the throughput tool on an MSMQ or Azure Storage Queues system. In order to successfully collect any data, every system endpoint must be modified to send data to the new ServiceControl installation or there will be no data to collect. Instead, reach out to a licensing specialist to suggest alternate ways to estimate the system throughput needed for licensing.

## Running the tool

Once installed, execute the tool with the URLs for the ServiceControl and monitoring APIs.

If the tool was [installed as a .NET tool](/nservicebus/throughput-tool/#installation-net-tool-recommended):

```shell
throughput-counter servicecontrol [options] --serviceControlApiUrl http://localhost:33333/api/ --monitoringApiUrl http://localhost:33633/
```

Or, if using the [self-contained executable](/nservicebus/throughput-tool/#installation-self-contained-executable):

```shell
Particular.EndpointThroughputCounter.exe servicecontrol [options] --serviceControlApiUrl http://localhost:33333/api/ --monitoringApiUrl http://localhost:33633/
```

Because ServiceControl contains, at maximum, the previous 1 hour of monitoring data, the tool will query the ServiceControl API 24 times with a one-hour sleep period between each attempt in order to capture a total of 24 hours worth of data.

For endpoints that do not have monitoring enabled, the tool will fall back to querying [message audit data](/nservicebus/operations/auditing.md) to determine how many messages have been processed each hour.

## Options

| Option | Description |
|-|-|
| <nobr>`--serviceControlApiUrl`</nobr> | **Required** – The URL of the ServiceControl API. In the [ServiceControl Management Utility](/servicecontrol/servicecontrol-instances/deployment/scmu.md), find the instance identified as a **ServiceControl Instance** and use the value of the **URL** field, as shown in the screenshot below. |
| <nobr>`--monitoringApiUrl`</nobr> | **Required** – The URL of the Monitoring API. In the [ServiceControl Management Utility](/servicecontrol/monitoring-instances/deployment/scmu.md), find the instance identified as a **Monitoring Instance** and use the value of the **URL** field, as shown in the screenshot below. |
include: throughput-tool-global-options

This screenshot shows how to identify the instance types and locate the required URLs:

![ServiceControl instances showing tool inputs](servicecontrol.png)

## What the tool does

The tool will send HTTP requests to both the [ServiceControl primary instance](/servicecontrol/servicecontrol-instances/) and the [ServiceControl monitoring instance](/servicecontrol/monitoring-instances/).

### Primary instance

The following requests will be sent to the primary instance:

* `<PrimaryUrl>`: Makes sure the URL is valid and that the ServiceControl version is compatible with the tool.
* `<PrimaryUrl>/endpoints`: Discovers endpoint names.
* `<PrimaryUrl>/configuration/remotes`: Discovers information about connected audit instances, and verifies that their versions are compatible with the tool.
* `<PrimaryUrl>/endpoints/{EndpointName}/audit-count`: Requested only once per endpoint, and retrieves throughput information for endpoints with auditing enabled.

### Monitoring instance

The following requests will be sent to the monitoring instance:

* `<MonitoringUrl>`: Makes sure the URL is valid and that the ServiceControl version is compatible with the tool.
* `<MonitoringUrl>/monitored-endpoints?history=60`: Retrieved once per hour to get throughput data for endpoints with monitoring enabled.
