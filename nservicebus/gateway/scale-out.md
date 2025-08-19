---
title: Scale out
summary: Information on scaling out the NServiceBus Gateway
reviewed: 2024-07-26
related:
 - samples/gateway
---

Due to specifics of the protocol used, the gateway is designed to run on at most one instance of each endpoint. Depending on the transport there are different strategies for designating a gateway hosting endpoint instance.

### Brokered transports - RabbitMQ, SQL Server, Azure and MSMQ scaled-out with unified scalability model

With brokered transports or bus transports with sender-side distribution, all instances are equal. All can host the gateway, but one instance must be explicitly selected as the receiver of the incoming HTTP gateway traffic. Any endpoints sending to the gateway of that endpoint must be configured to use the HTTP address of the selected endpoint instance.

Use an HTTP load balancer to avoid hard-coding individual endpoint instance gateway HTTP addresses in the gateway senders. Configure sending endpoints to send to the load balancer and let the load balancer forward the traffic to the HTTP address of the selected endpoint instance.

Handle high availability requirements by setting the load balancer to fail over to another endpoint instance if it detects endpoint failure.

![Gateway with Version 6 scaleout](/nservicebus/gateway/scaleoutv6.png 'width=400')

## Caveats

[Callbacks](/nservicebus/messaging/callbacks.md) through the gateway are not supported on scaled-out endpoints.
