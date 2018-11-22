---
title: Selecting a transport
summary: The differences between each transport and how to decide which one to take.
component: Core
tags:
 - Transport
reviewed: 2018-11-01
---

NServiceBus is an abstraction of various queuing technologies, which we call transports. This allows development teams to build a system with NServiceBus and the Particular Platform on top of an already selected transport. Or to select the right transport for your needs. This way you can focus on building features instead of deep diving into the details of any transport.

This document provides guidance on selecting the right transport for generic scenarios. There is even a decision chart that might help guide the decision.

Through documentation, it is impossible to provide a clear path for every single possible scenario. Every decision has multiple trade-offs, some of which we added to the documentation in each transport. The result is that this guide is not a definitive answer for everyone. If you’re still unclear of what the best choice is, or you have very specific requests, don’t hesitate to contact us via support or chat.

![transport-decision-chart](/transports/transport-decision-chart.png)

## Broker versus Store & Forward

Most transports have a centrally located message broker that takes care of the physical message routing, etc. This means that the messaging infrastructure is installed centrally and applications connect to that message broker. If the message broker cannot be reached, the system is unable to send messages. It is therefore important that this message broker is highly available.

One transport works via store & forward, which is MSMQ. This is the only transport that is running locally on every machine that needs access to the messaging infrastructure. The result is that an application that wants to send a message, has access to this infrastructure, even if the network is down. The benefit is that message can always be sent, even though they won’t be delivered until the connection to other machines is restored again.

For information on the difference between a message broker and a service broker, visit our [bus vs broker architecture page](https://docs.particular.net/nservicebus/architecture/).

## The different transports

Below are the advantages and disadvantages of our supported transports, including a section on why you might want to choose that transport.

- Learning Transport
- MSMQ
- Azure Service Bus
- Azure Storage Queues
- SQL Server
- RabbitMQ
- Amazon SQS

For every cloud hosted transport, it is possible that the network connection to the cloud provider might suffer. The result is that you cannot submit new messages and can possibly result in lost data from, for example, a user interface. If this user interface is also running at the same datacenter, this is less likely to occur.

## Learning Transport

This transport should not be used in production!

It has benefits for learning how to work with NServiceBus. Instead of requiring additional technology installed, it will work straight out of the box. This is achieved by the fact that this transport stores messages as files on disk which can be read by other endpoints.



## MSMQ Transport

The MSMQ transport uses native Windows queuing mechanism, MSMQ, to deliver messages. MSMQ is a distributed system that consists of multiple processes, one on each machine. The client always interacts with a process running locally and the messages are forwarded in the background.

#### Advantages

- A built-in component of Windows operating system
- Supports distributed transactions allowing atomic message receive and data manipulation through [Microsoft Distributed Transaction Coordinator (MSDTC)](https://msdn.microsoft.com/en-us/library/ms684146.aspx)
- Provides a store and forward mechanism which allows endpoints to send messages even though the destination endpoint might be temporarily unreachable due to network issues.

#### Disadvantages

- Does not offer a native publish-subscribe mechanism, therefore it requires NServiceBus persistence to be configured for storing event subscriptions. [Explicit routing for publish/subscribe](/nservicebus/messaging/routing.md#event-routing-message-driven) must also be specified.
- Many organizations don't have the same level of operational expertise with MSMQ that they do with other technologies (e.g. SQL Server), so it may require additional training.
- Scaling out compute-intensive workload require setting up the distributor or sender-side distribution of messages to destination queues. This is harder than with the competing consumer pattern that other transports support.

#### When would you select this transport

- If you need store & forward for a higher reliability that your queuing technology is always available.
- If you’re running a Windows environment on-premise and don’t want to spend additionally on licenses and/or training.
- If you are able to coop distributed transactions and want the reliability it adds.



## Azure Service Bus Transport

Azure provides multiple messaging technologies and one of the most advanced and reliable ones is Azure Service Bus.

## Advantages

- Fully managed turn-key infrastructure
- Ability to scale in both features and price range.
- AMQP based implementation
- Supports messaging transactions unlike other messaging services on Azure
- Up to 1MB message size
- Levering native capabilities of the transport instead of taxing an endpoint, like publish/subscribe and message deferral

## Disadvantages

- Requires monitoring of costs, although NServiceBus supports tweaking number of transactions to Azure.
- No on-premise option for development or testing
- Processing a message has to complete within 5 minutes
- Relies on TCP and might require opening additional ports in the firewall

#### When would you select this transport

- If your application is running on Windows Azure
- If you want enterprise messaging features, like additional reliability
- If you want a bigger message size than Azure Storage Queues
- If you want to scale up and out with your business needs



## Azure Storage Queues

Azure Storage Queues is a service hosted on the Azure platform. Compared to Azure Service Bus it has less advanced features but is also more cost effective.

#### Advantages

- Fully managed turn-key infrastructure
- Can store very large numbers of messages (up to 200 TB limit of the Azure Storage account). Even though you should not store that many messages, it is a feature we wanted to add.
- Very low price per message
- Very high availability

#### Disadvantages

- The size of a single message is limited to 64 KB including headers and body. NServiceBus uses headers for metadata, which could consume a lot of the message overall size.
- Does not offer a native publish-subscribe mechanism, therefore it requires NServiceBus persistence to be configured for storing event subscriptions. [Explicit routing for publish/subscribe](/nservicebus/messaging/routing.md#event-routing-message-driven) must also be specified
- Significant latency when receiving messages due to HTTP-based polling communication protocol

#### When would you select this transport

- If you’re running in Windows Azure and don’t want the additional cost or features of Azure Service Bus
- If you don’t have a high throughput of messages, which would require Azure Service Bus to scale out.



## SQL Server Transport

The SQL Server Transport implements queues on top of relational tables. Each row of the table holds one message consisting of ID, headers and body plus some additional columns for backward compatibility.

#### Advantages 

- No additional licensing and training costs; many Microsoft stack organizations have SQL Server installed and have the knowledge required to run it
- Mature tooling, such as [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Free to start with the [SQL Server Express edition](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express).
- Easy scale-out through competing consumers -- multiple instances of the same endpoint consuming messages from a single queue
- Supports atomic message receive and data manipulation through [Microsoft Distributed Transaction Coordinator (MSDTC)](<https://msdn.microsoft.com/en-us/library/ms684146.aspx>).
- Supports both queues and business data in a single backup, making it much easier to restore your system into a consistent state.

#### Disadvantages

- Does not offer a native publish-subscribe mechanism, therefore it requires NServiceBus persistence to be configured for storing event subscriptions. [Explicit routing for publish/subscribe](/nservicebus/messaging/routing.md#event-routing-message-driven) must also be specified.
- Combined messages throughput of all endpoints adds additional load on your SQL Server installation.
- The transport polls the queue table even if there are no messages to process.

#### When would you select this transport

- If you cannot introduce another queuing technology
- If you have invested in a SQL Server cluster
- If you don’t want to invest in possible training for another queuing technology.
- If you have a legacy application and need SQL triggers for integration with NServiceBus.



## RabbitMQ Transport

RabbitMQ is a popular message broker that is used on many development platforms. It can be used both on-premise and in the cloud.

#### Advantages

- There is a large community around RabbitMQ
- Provides native reliability and high-availability features.
- Offers native publish-subscribe features, therefore doesn’t need NServiceBus persistence.
- Allows for integration with applications written in other languages, using native RabbitMQ features through a wide range of [supported clients](<https://www.rabbitmq.com/devtools.html>).
- Supports [competing consumer pattern](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html) out of the box. Messages are received by instances in a round-robin fashion without additional configuration.

#### Disadvantages

- Running RabbitMQ in a cluster, which is highly recommended, requires deeper operational knowledge of RabbitMQ. Not all companies have the same level of expertise as with other technologies, like SQL Server. This may require additional training.
- Doesn’t handle [network partitions](https://www.rabbitmq.com/partitions.html) well; partitioning across a WAN requires using dedicated features
- Requires careful consideration for duplicate messages, e.g. using the outbox feature or making all endpoints idempotent.
- Might require covering additional costs of [commercial RabbitMQ license and support](<https://www.rabbitmq.com/services.html>).

#### When would you select this transport

- If you want native integration with other development platforms
- If you already are running RabbitMQ and don’t want another queuing technology

### Amazon SQS

This is a popular transport if your system is hosted inside AWS, Amazon its cloud offering.

#### Advantages

- Fully managed turn-key infrastructure
- Automatically scales up and down

#### Disadvantages

- Might be relatively expensive in a high throughput scenario
- Within the Microsoft and .NET world it’s less common to use this than other transports. This might mean you need to reach out to unknown resources for support.

#### When would you select this transport

- If your system is hosted inside AWS
- If you are already using Amazon SQS and/or want native integration from other existing systems