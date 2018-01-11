---
title: Heartbeats
summary: 
reviewed: 2018-01-05
component: Heartbeats
versions: 'Heartbeats:*'
---

The Heartbeat plugin enables endpoint health monitoring by sending regular heartbeat messages from the endpoint to a ServiceControl instance. The ServiceControl instance keeps track of which endpoint instances are sending heartbeats and which ones are not. 

NOTE: Even if an endpoint is able to send heartbeat messages, other failures may occur within the endpoint and its host that prevent it from performing as expected. For example, the endpoint may not be able to process incoming messages, or it may be able to send messages to the ServiceControl queue but not another queue.

```mermaid
graph LR
	
subgraph Endpoint
Heartbeats
end
	
Heartbeats -- Heartbeat<br>Data --> SCQ

SCQ[ServiceControl<br>Input Queue] --> SC[ServiceControl]

SC -. Integration<br/>Events .-> Integration[Integration<br/>Event Handler]

SC -- Endpoint health<br>data --> ServicePulse
```


## Set up Heartbeats

To enable heartbeat monitoring in an environment:

1. [Install a ServiceControl instance](/servicecontrol/servicecontrol-instances/)
2. [Install and configure the Heartbeat plugin in endpointsthat need to be monitored](install-plugin.md)
3. [View the status of monitored endpoints in ServicePulse](in-servicepulse.md)
4. Optionally [subscribe to integration events from ServiceControl when endpoints start/stop heartbeating](notification-events.md)

When an endpoint starts sending heartbeat messages, the ServiceControl instance will mark the endpoint as "active". If the ServiceControl instance stops receiving heartbeat messages from an endpoint, it will mark that endpoint as "inactive". 

The heartbeats plugin is only able to determine if the endpoint is running and sending heartbeat messages. For more sophisticated endpoint health checks, develop a [Custom Check](/monitoring/custom-checks/)
