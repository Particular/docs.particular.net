---
title: Monitoring
summary: Monitoring
reviewed: 2018-01-03
---

The endpoints of an NServiceBus system collect different types of information about their operation. This information can be collected and aggregated into a monitoring solution for the whole system.


## Endpoint performance

Endpoints collect information about how well they are processing messages. This data can be [sent to ServiceControl Monitoring for visualization in ServicePulse](nservicebus/operations/metrics/service-control). 

Use this data to understand [which message handlers need to be optimized](tutorials/monitoring-demo/walkthrough-1), [which endpoints need to be scaled out](tutorials/monitoring-demo/walkthrough-2), and [where there are recoverable errors slowing the system down](tutorials/monitoring-demo/walkthrough-3).


## Endpoint health

ServiceControl can track which endpoint instance are running by listening to a [continual stream of heartbeat messages](nservicebus/operations/heartbeat). Endpoints that fail to send heartbeats within a defined period of time are considered to be offline and an [alert is raised](servicecontrol/contracts) to an operator.

Additionally, endpoints can include [custom code which runs on a regular schedule and reports status to ServiceControl](nservicebus/operations/custom-checks). Custom Checks can be used to ensure that 3rd party web services and applications are running as expected.