---
title: "Monitoring NServiceBus solutions: Configuring endpoints"
reviewed: 2017-10-10
summary: Setting up NServiceBus endpoints to be monitored by the Particular Service Platform.
extensions:
- !!tutorial
  nextText: "Next Lesson: Throughput and processing time"
  nextUrl: tutorials/monitoring/4-endpoint-performance
---

include: monitoring-intro-paragraph

include: monitoring-sample-solution

This third lesson guides you through setting up all of the endpoints in your system to be monitored by the Particular Software Platform.

NOTE: If you are using the sample monitoring solution, each endpoint contains placeholder comments in it's `Program.cs` for each of the snippets presented below.


## Configure endpoints

In order to be monitored by the Particular Service Platform, each endpoint needs to be configured to send data to three different queues: error, audit, and monitoring.

It is recommended to go through this entire process for a single endpoint and ensure that it is working before continuing on to the next endpoint.

NOTE: If you are setting up the sample monitoring solution, start with the Sales endpoint. 


### Error

Whenever an NServiceBus endpoint is unable to process a message (even after several attempts) it will forward a copy to the error queue. Each environment should contain a single error queue and should not share an error queue with another environment.

The error queue is created with a new ServiceControl instance. By default, it is named `error`.

You can configure the location of the error queue. Add the following code to your endpoint configuration:

```csharp
endpointConfiguration.SendFailedMessagesTo("error");
```

NOTE: If you are using the MSMQ transport then you also need to specify the machine where the error queue is located. i.e. `error@MACHINENAME`. This should be the same machine where your ServiceControl instance is installed.


### Audit

Whenever an NServiceBus endpoint successfully processes a message, it can be configured to send a copy to a centralized audit queue. Each environment should contain a single audit queue and should not share an audit queue with another environment.

The audit queue is created with a new ServiceControl instance. By default, it is named `audit`.

You can enable auditing and configure the location of the audit queue. Add the following code to your endpoint configuration:

```csharp
endpointConfiguration.AuditProcessedMessagesTo("audit");
```

NOTE: If you are using the MSMQ transport then you also need to specify the machine where the audit queue is located. i.e. `audit@MACHINENAME`. This should be the same machine where your ServiceControl instance is installed.


### Monitoring

An NServiceBus endpoint can be configured to send data about it's health and performance to a centralized monitoring queue. Each environment should contain a single monitoring queue and should not share a monitoring queue with another environment. 

The monitoring queue is created with a new Monitoring instance. By default, it is named `Particular.Monitoring`.

To get an NServiceBus endpoint to send metric data to the monitoring queue you need to install the ServiceControl metrics package:

```ps
Install-Package NServiceBus.Metrics.ServiceControl
```

NOTE: This will also install the **NServiceBus.Metrics** package.

You can then enable monitoring and configure the location of the monitoring queue. Add the following code to your endpoint configuration:

```csharp
var metrics = endpointConfiguration.EnableMetrics();

metrics.SendMetricDataToServiceControl(
    serviceControlMetricsAddress: "Particular.Monitoring", 
    interval: TimeSpan.FromSeconds(10)
);
```

NOTE: If you are using the MSMQ transport then you also need to specify the machine where the monitoring queue is located. i.e. `Particular.Monitoring@MACHINENAME`. This should be the same machine where your Monitoring instance is installed.


## Smoke Test

Now that everything is installed, run the system and open up ServicePulse. In the monitoring tab you should see a list of running endpoints.

SCREENSHOT - ServicePulse Monitoring tab - list of endpoints happily sending data