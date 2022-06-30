---
title: Use OpenTelemetry with Prometheus and Grafana
summary: Use OpenTelemetry with Prometheus and Grafana to capture and visualize NServiceBus metrics.
component: Core
isLearningPath: true
reviewed: 2022-06-24
previewImage: grafana.png
---


## Introduction

[Prometheus](https://prometheus.io) is a monitoring solution for storing time series data like metrics. [Grafana](https://grafana.com) allows to visualize the data stored in Prometheus (and other sources). This sample demonstrates how to capture NServiceBus OpenTelemetry metrics, store them in Prometheus and visualize these metrics using a Grafana dashboard.


![Grafana NServiceBus fetched, processed and errored messages](grafana.png)

## Prerequisites

To run this sample, Prometheus and Grafana are required. This sample uses docker and an accompanied `docker-compose.yml` file to run the stack.

## Code overview

The sample simulates messages load with a random 10% failure rate using the `LoadSimulator` class:

snippet: prometheus-load-simulator

## Reporting metric values
NServiceBus version 8.x uses OpenTelemetry standard to report metrics. The metrics are disabled by default, and can be enabled:

snippet: enable-opentelemetry-metrics

There are three metrics reported as a Counter, with the following keys:

 * Number of fetched messages via `messaging.fetches`
 * Number of failed messages via `messaging.failures`
 * Number of successfully processed messages via `messaging.successes`

Each reported metric is tagged with the following additional information:

 * `messaging_queue` the queue name of the endpoint
 * `messaging_discriminator` a uniquely addressable address for the endpoint (discriminator when scaling out)
 * `messaging_type` the .NET fully-qualified type information for the message being processed
 * `messaging_failure_type` the exception type name (if applicable)

## Exporting metrics

The metrics are gathered using OpenTelemetry standard on the endpoint and need to be reported and collected by an external service. A Prometheus exporter can expose this data via an HTTP endpoint and the Prometheus service, hosted as a docker service, can retireve and store these information. This exporter is available via a NuGet package `OpenTelemetry.Exporter.Prometheus`. In this sample the service that exposes the data to scrape is hosted on `http://localhost:9185/metrics`. To enable the Prometheus exporter, the following should be executed:

snippet: enable-prometheus-exporter

Note: that the HTTP endpoint is also exposed through a local IP address so the Prometheus service running in docker can reach it over the network.

The raw metrics retrieved through the scraping endpoint would look like this:

```text
# HELP messaging_successes Total number of messages processed successfully by the endpoint.
# TYPE messaging_successes counter
messaging_successes{messaging_discriminator="main",messaging_queue="Samples.OpenTelemetry.Metrics",messaging_type="SomeCommand, Otel.MetricsDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"} 526 1656039256653

# HELP messaging_fetches Total number of messages fetched from the queue by the endpoint.
# TYPE messaging_fetches counter
messaging_fetches{messaging_discriminator="main",messaging_queue="Samples.OpenTelemetry.Metrics",messaging_type="SomeCommand, Otel.MetricsDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"} 643 1656039256653

# HELP messaging_failures Total number of messages processed unsuccessfully by the endpoint.
# TYPE messaging_failures counter
messaging_failures{messaging_discriminator="main",messaging_failure_type="System.Exception",messaging_queue="Samples.OpenTelemetry.Metrics",messaging_type="SomeCommand, Otel.MetricsDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"} 117 1656039256653
```

Below diagram shows the overall component interactions:

```mermaid
graph TD
    A[NServiceBus Endpoint] -->|Report Metrics| B(Prometheus Exporter)
    B -->|Expose| C{Metric Endpoint}
    C -->|No Metrics| D[Status 200]
    C -->|Has Metrics| E[Return Metrics]
    F[Promethus Service] --> |Poll Metrics| E
    F --> |Store Metrics| F
    G[Grafana] --> |Query Data| F
```

## Docker stack

Prometheus service needs to be configured to get the metrics data from the endpoint. Grafana also needs to be configured to get the data from Promethus and visualize it as graphs.

To run the docker stack, run `docker-compse up -d` in the directory where the `docker-compose.yml` file is located.

### Configuring Prometheus

Copy the following files into the mapped volumes of the Prometheus and Grafana.

 * `prometheus_ds.yml` should be copied to `./grafana/provisioning/datasources` folder
 * `prometheus.yml` should be copied to `./prometheus` folder

Open `prometheus.yml` and update the target IP address. This should be the address of the machine running the sample and the port the Promethus exporter is configured to run. Docker stack should be able to reach this IP and port. 

```yml
  - targets:
    - '192.168.0.10:9184'
```

### Show a graph

Start Prometheus by running the docker stack. NServiceBus pushes events for *success, failure, and fetched*. These events need to be converted to rates by a query, for example the `messaging_successes` metric can be queried as:

```
avg(rate(messaging_successes[5m]))
```

![Prometheus graphs based on query](example-prometheus-graph.png)

## Grafana

Grafana needs to be installed and configured to display the data scraped and stored in Prometheus. For more information how to install Grafana refer to the [Installation Guide](https://docs.grafana.org/installation). In this sample, the Grafana service runs as part of the docker stack mentioned above.

#### Dashboard

To graph the metrics, the following steps have to be performed:

 * Add a new dashboard
 * Add a graph
 * Click its title to edit
 * From the Data source dropdown select Prometheus
 * For the query open the Metrics dropdown and select one of the metrics. Built-in functions (e.g. rate) can also be applied.

<!-- ![Grafana metric using Prometheus as datasource](grafana-metric.png) -->

![Grafana dashboard with NServiceBus OpenTelemetry metrics](example-grafana-dashboard.png)

The sample includes an [export of the grafana dashboard](grafana-endpoints-dashboard.json) which can be [imported](https://docs.grafana.org/reference/export_import/) as a reference.
