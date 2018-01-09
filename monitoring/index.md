---
title: Monitoring
reviewed: 2018-01-03
---

The endpoints of an NServiceBus system collect different types of information about their operation. This information can be collected and aggregated into a monitoring solution for the whole system.


## Endpoint performance

Endpoints collect information about how well they are processing messages. This data can be [sent to ServiceControl Monitoring for visualization in ServicePulse](/nservicebus/operations/metrics/service-control.md). 

Use this data to understand [which message handlers need to be optimized](/tutorials/monitoring-demo/walkthrough-1.md), [which endpoints need to be scaled out](/tutorials/monitoring-demo/walkthrough-2.md), and [where there are recoverable errors slowing the system down](/tutorials/monitoring-demo/walkthrough-3.md).


## Endpoint health

A ServiceControl instance can track which endpoint instance are running by listening to a [continual stream of heartbeat messages](/monitoring/heartbeats/). Endpoints that fail to send heartbeats within a defined period of time are considered to be inactive. When an inactive endpoint starts sending heartbeats again, it becomes active. 

Beyond heartbeats, an endpoint can include [custom code to check anything and report status](/monitoring/custom-checks/). These checks can be run on a regular schedule and can be be used to ensure that 3rd party web services and applications are running as expected.

ServicePulse can be used to [track which endpoints are active and inactive](/monitoring/heartbeats/in-servicepulse.md) as well as [monitor the status of Custom Checks](/monitoring/custom-checks/in-servicepulse.md).

When an endpoint becomes active or inactive an [event is raised](/monitoring/heartbeats/notification-events.md) enabling integration with any other dashboard or tool. [Events are also raised when Custom Checks change status](/monitoring/custom-checks/notification-events.md).
