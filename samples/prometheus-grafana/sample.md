---
title: Capture and visualize metrics using Prometheus and Grafana
component: Metrics
---

[Prometheus](https://prometheus.io) is a monitoring solution for storing time series data like metrics. [Grafana](https://grafana.com) allows to query and visualize the data stored in Prometheus (and other sources). This sample demonstrates how to capture NServiceBus metrics, storing these in Prometheus and visualizing this using Grafana.



![Grafana NServiceBus processing time](grafana-graph.png)



This sample reports the following metrics to Prometheus:

 * Fetched messages per second 
 * Failed messages per second
 * Successful messages per second
 * Critical time 
 * Processing time



 The sample also shows data like `Average message processing time in milliseconds` and `Maximum critical time in milliseconds`. For a detailed explanation of these metrics refer to the [metrics captured section in the metrics documentation](/nservicebus/operations/metrics.md#metrics-captured).


## Prerequisites

To run this sample, download and run both Prometheus and Grafana. This sample uses Prometheus 1.7.1 and Grafana 4.4.2.

- https://prometheus.io
- https://grafana.com


## Code overview

The sample simulates messages load with a random 10% failure rate using the `LoadSimulator` class:

snippet: prometheus-load-simulator


## Capturing metric values

The following nuget packages are required:

- NServiceBus.Metrics
- prometheus-net

A Prometheus service is hosted inside the endpoint via the nuget package `prometheus-net`. The service enables Prometheus to scrape data gathered by the metrics package. Custom observers need to be registered for the metric probes provided via `NServiceBus.Metrics`. This is all setup in  the `PrometheusFeature`


snippet: prometheus-enable-nsb-metrics


The names provided by the `NServiceBus.Metrics` probes are not compatible with Prometheus. Thus the names need to be aligned with the [naming conventions defined by Prometheus](https://prometheus.io/docs/practices/naming/) by mapping them accordingly

Counters : `nservicebus_{counter-name}_total`

Summaries: `nservicebus_{summary-name}_seconds`


snippet: prometheus-name-mapping


The registered observers convert NServiceBus.Metric *Signals* to Prometheus *Counters* and NServiceBus.Metric *Durations* to Prometheus *Summaries*.  Additionally, labels are added that identify the endpoint, the endpoint queue and more within Prometheus. With this label, it is possible to filter and group metric values. 

snippet: prometheus-register-probe

WARNING: Labels should be chosen thoughtfully since each unique combination of key-value label pairs represents a new time series which can dramatically increase the amount of data stored. The labels used here are for demonstration purpose only.

During the registration the following steps are required:

- Map metric names
- Register observer callbacks
- Create summaries and counters with corresponding labels
- Invoke the summaries and counters in the observer callback

snippet: prometheus-observers-registration


## Prometheus

Prometheus needs to be configured to pull data from the endpoint. For more information how to setup Prometheus refer to the [getting started guide](https://prometheus.io/docs/introduction/getting_started/). 

### Guided configuration

Copy

- [nservicebus.rules](nservicebus.rules)
- [prometheus.yml](prometheus.yml)

into the root folder of the Prometheus installation. Overwrite the existing `prometheus.yml` in the Prometheus demo installation. Or proceed with the manual configuration if desired.

### Manual configuration

#### Add a target

Edit `prometheus.yml` and  add a new target for scraping similar to

```
- job_name: 'nservicebus'

    scrape_interval: 5s

    static_configs:
      - targets: ['localhost:3030']
```

#### Define rules

Queries can be expensive operations. Prometheus allows defining pre calculated queries by configuring rules that calculate rates based on the counters. 

    nservicebus_success_total:avg_rate5m = avg(rate(nservicebus_success_total[5m]))
    nservicebus_failure_total:avg_rate5m = avg(rate(nservicebus_failure_total[5m]))
    nservicebus_fetched_total:avg_rate5m = avg(rate(nservicebus_fetched_total[5m]))


The precalculated query can then be used.

    nservicebus_success_total:avg_rate5m

To leverage the power of rules which are required by the sample dashboard shown later create `nservicebus.rules` in the root folder of the Prometheus installation and add the three rules as defined above.

To enable the rules edit `prometheus.yml` and add:

```
rule_files:
  - 'nservicebus.rules'
```

### Show a graph

Start Prometheus and open `http://localhost:9090` in a web browser.

NServiceBus pushes events for *success, failure, and fetched*. These events need to be converted to rates by a query:

    avg(rate(nservicebus_success_total[5m])) 

![Prometheus graphs based on query](example-prometheus-graph.png)

### Example configuration

Prometheus configuration files demonstrating the concepts from this sample:

- [nservicebus.rules](nservicebus.rules)
- [prometheus.yml](prometheus.yml)


## Grafana

Grafana needs to be installed and configured to display the data available in Prometheus. For more information how to install Grafana refer to the [Installation Guide](http://docs.grafana.org/installation).


### Guided configuration

Execute `setup.grafana.ps1` in a powershell with elevated permission and provide the username and password to authenticate with Grafana. 


### Manual configuration

#### Datasource

Create a new data source called `PrometheusNServiceBusDemo`. For more information how to define a Prometheus data source refer to [Using Prometheus in Grafana](http://docs.grafana.org/features/datasources/prometheus/).

#### Dashboard

To graph the Prometheus rule  `nservicebus_failure_total:avg_rate5m` the following steps have to be performed:

- Add a new dashboard 
- Add a graph
- Click its title to edit
- Click the Metric tab

![Grafana metric using Prometheus as datasource](grafana-metric.png)

### Dashboard

![Grafana dashboard with NServiceBus metrics](example-grafana-dashboard.png)

The sample included an [export of the grafana dashboard](grafana-endpoints-dashboard.json), this can be [imported](http://docs.grafana.org/reference/export_import/) as a reference.