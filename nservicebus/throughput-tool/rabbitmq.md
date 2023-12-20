---
title: Measuring system throughput using RabbitMQ
summary: Use the Particular throughput tool to measure the throughput of an NServiceBus system.
reviewed: 2022-11-09
related:
  - nservicebus/throughput-tool
---

The Particular throughput tool can be installed locally and run against a production system to discover the throughput of each endpoint in a system over a period of time.

This article details how to collect endpoint and throughput data when the system uses the [RabbitMQ transport](/transports/rabbitmq/). Refer to the [throughput counter main page](./) for information how to install/uninstall the tool or for other data collection options.

## Running the tool

To collect data from RabbitMQ, the [management plugin](https://www.rabbitmq.com/management.html) must be enabled on the RabbitMQ broker. The tool will also require a login that can access the management UI.

Execute the tool, providing the RabbitMQ management URL, as in this example where the RabbitMQ broker is running on localhost.

If the tool was [installed as a .NET tool](/nservicebus/throughput-tool/#installation-net-tool-recommended):

```shell
throughput-counter rabbitmq [options] --apiUrl http://localhost:15672
```

Or, if using the [self-contained executable](/nservicebus/throughput-tool/#installation-self-contained-executable):

```shell
Particular.EndpointThroughputCounter.exe rabbitmq [options] --apiUrl http://localhost:15672
```

The tool will prompt for the username and password to access the RabbitMQ management interface. After that, it will take its initial reading, then sleep for 24 hours before taking its final reading and generating a report.

### Options

| Option | Description |
|-|-|
| <nobr>`--apiUrl`</nobr> | **Required** – The URL for the RabbitMQ management site. Generally this will be `http://<rabbitmq-hostname>:15672` |
| <nobr>`--vhost`</nobr> | The [RabbitMQ virtual host](https://www.rabbitmq.com/vhosts.html) to measure. Only required if the broker has multiple vhosts.<br/><br/>Example: `--vhost prod` |
include: throughput-tool-global-options

## What the tool does

After performing an interactive login to the RabbitMQ Management API, the tool will:

1. Query `/api/overview` to get basic information about the system, ensure that the current version of RabbitMQ is compatible with the tool, and that the necessary plugins are installed to be able to get queue throughput details.
2. Query `/api/queues?page=1&page_size=500&name=&use_regex=false&pagination=true` to discover queue names and to get the message stats for each queue. If more than 500 queues are present, the additional pages of data will be requested as well.
3. The queries to `/api/queues` will be repeated every 5 minutes over the 24-hour tool runtime. This way, the tool can continue counting if a restart of a RabbitMQ server instance causes the message stats to reset.
