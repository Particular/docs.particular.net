---
title: 'Gateway: Scale-Out'
summary: How the Gateway handles Scale-Out
reviewed: 2016-03-17
related:
 - samples/gateway
---

Due to specifics of the protocol used, the gateway is designed to run on at most one instance of each endpoint. Depending on the transport there are different strategies for designating a gateway hosting endpoint instance:


### MSMQ scaled-out with the distributor

When scaling out MSMQ through the [distributor](/nservicebus/msmq/scalability-and-ha/distributor), the node running the distributor is the natural candidate to host the gateway. Running the distributor as [a clustered service on a Windows Failover Cluster](/nservicebus/msmq/scalability-and-ha/deploying-to-a-windows-failover-cluster.md) provides High Availability (HA). No special action needs to be taken, other than configuring senders to send gateway HTTP traffic to the node running the distributor instead of directly to the workers.

![Gateway with distributor](/nservicebus/gateway/scaleoutdistributor.png)


### Brokered transports - RabbitMQ, SQL Server, Azure and MSMQ scaled-out with new unified scalability model in version 6

With [brokered transports](/nservicebus/scalability-and-ha/scale-out.md#versions-5-and-below-sql-server-and-rabbitmq) or the [new unified scalability model](/nservicebus/scalability-and-ha/scale-out.md#versions-6-and-above) in version 6, all instances are equal. All can host the gateway, but one of them needs to be explicitly selected as the receiver of the incoming HTTP gateway traffic. Any endpoints sending to the gateway of that endpoint must be configured to use the HTTP address of the selected endpoint instance.

Use an HTTP Load Balancer(LB) to avoid hard-coding individual endpoint instance gateway HTTP addresses in the gateway senders. Configure sending endpoints to send to the LB and let the LB forward the traffic to the HTTP address of the selected endpoint instance.

Handle HA requirements by setting the LB to fail over to another endpoint instance if it detects endpoint failure.

![Gateway with Version 6 scaleout](/nservicebus/gateway/scaleoutv6.png)


## Caveats

[Callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) through the gateway are not supported on scaled-out endpoints.