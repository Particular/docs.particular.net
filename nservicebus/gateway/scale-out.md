---
title: Scale out
reviewed: 2019-07-18
related:
 - samples/gateway
---

Due to specifics of the protocol used, the gateway is designed to run on at most one instance of each endpoint. Depending on the transport there are different strategies for designating a gateway hosting endpoint instance.


### MSMQ scaled-out with the distributor

When scaling out MSMQ through the [distributor](/transports/msmq/distributor), the node running the distributor is the natural candidate to host the gateway. Running the distributor as [Failover Cluster](https://technet.microsoft.com/en-us/library/dn505754.aspx) provides High Availability (HA). No special action needs to be taken, other than configuring senders to send gateway HTTP traffic to the node running the distributor instead of directly to the workers.

![Gateway with distributor](/nservicebus/gateway/scaleoutdistributor.png 'width=400')


### Brokered transports - RabbitMQ, SQL Server, Azure and MSMQ scaled-out with new unified scalability model in version 6

With brokered transports or bus transports with sender-side distribution, all instances are equal. All can host the gateway, but one of them needs to be explicitly selected as the receiver of the incoming HTTP gateway traffic. Any endpoints sending to the gateway of that endpoint must be configured to use the HTTP address of the selected endpoint instance.

Use an HTTP Load Balancer(LB) to avoid hard-coding individual endpoint instance gateway HTTP addresses in the gateway senders. Configure sending endpoints to send to the LB and let the LB forward the traffic to the HTTP address of the selected endpoint instance.

Handle HA requirements by setting the LB to fail over to another endpoint instance if it detects endpoint failure.

![Gateway with Version 6 scaleout](/nservicebus/gateway/scaleoutv6.png 'width=400')


## Caveats

[Callbacks](/nservicebus/messaging/callbacks.md) through the gateway are not supported on scaled-out endpoints.