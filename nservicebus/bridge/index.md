---
title: Bridge
summary: How to connect endpoints in a system that use different transports
reviewed: 2022-04-01
component: Bridge
---

The `NServiceBus.Transport.Bridge` allows NServiceBus endpoints to connect to other endpoints that are not on the same transport.

The bridge is transparent to sending and receiving endpoints.

- Endpoints send and receive messages to and from logical endpoints as if there was no bridge involved.
- Endpoints are not aware a bridge is transferring messages across transports.

## Why use the bridge

The bridge needs to be made aware of endpoints that exist on any supported transport through configuration. The bridge will then start collecting and processing messages for each endpoint that is on another transport and transfers these messages to this specific endpoint, bridging the transport.

This enables the following scenarios:

- Migration from one transport to another  
  For example migration from MSMQ to a cloud-native transport to remove the older MSMQ and start using .NET instead of .NET Framework
- Use multiple transport because of pricing considerations  
  Instead of *only* relying on a more expensive cloud-native transport, move only the less mission critical endpoints to a cheaper transport.
- Use transport that best fits non-functional considerations
  Use a transport that allows bridging geographical locations, while some endpoints on a more secure transport only process more private information.

## Bridge configuration

`NServiceBus.Transport.Bridge` is packaged as a host-agnostic library. It can be hosted e.g. inside a console application, a Windows service or a Docker container.

The following snippet shows a simple MSMQ-to-AzureServiceBus configuration.

snippet: bridgeconfiguration

The life cycle of the bridge is managed by the .NET Generic Host.

## Consistency

Since the bridge moves messages across different transport types or different brokers of the same type (Azure ServiceBus namespaces, RabbitMQ vhosts etc) the only transaction mode that can be supported is [`ReceiveOnly`](/transports/transactions.md#transactions-transport-transaction-receive-only). In this mode messages being moved across can be duplicated if some infrastructure related issue prevents the message from being moved to the target transport. To address this [either handlers have to be made idemotent or deduplication using the outbox have to be enabled](/transports/transactions.md#transactions-transport-transaction-receive-only-consistency-guarantees).

The bridge will use this mode if at least one of the transports being configured are not able to use ditributed transactions. (see below)

### Distributed transactions

If all configured transports supports the [`TransactionScope`](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) transaction mode the bridge default make use of it to make sure that no messages are duplicated during message transfer between transports. This enables migration from MSMQ to SqlServer without having to make changes to any of the involved endpoints.

Should `ReceiveOnly` transaction mode be prefered instead, use the following configuration:

snippet: bridge-configuration-explicit-receive-only-mode
