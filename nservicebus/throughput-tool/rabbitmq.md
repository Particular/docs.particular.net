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

Execute the tool with the RabbitMQ management URL, as in this example where the RabbitMQ broker is running on localhost:

```shell
throughput-counter rabbitmq --apiUrl http://localhost:15672
```

The tool will prompt for the username and password to access the RabbitMQ management interface. After that, it will take its initial reading, then sleep for 24 hours before taking its final reading and generating a report.

### Options

| Option | Description |
|-|-|
| <nobr>`--apiUrl`</nobr> | **Required** – The URL for the RabbitMQ management site. |
include: throughput-tool-global-options