---
title: Bridge
summary: TBD
reviewed: 2022-04-12
component: Bridge
---

## TBD

TBD

## Consistency

Since the bridge moves messages across different transport types or different brokers of the same type (Azure ServiceBus namespaces, RabbitMQ vhosts etc) the only transaction mode that can be supported is [`ReceiveOnly`](/transports/transactions.md#transactions-transport-transaction-receive-only). In this mode messages being moved across can be duplicated if some infrastructure related issue prevents the message from being moved to the target transport. To address this [either handlers have to be made idemotent or deduplication using the outbox have to be enabled](/transports/transactions.md#transactions-transport-transaction-receive-only-consistency-guarantees).

The bridge will use this mode if at least one of the transports being configured are not able to use ditributed transactions. (see below)

### Distributed transactions

If all configured transports supports the [`TransactionScope`](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) transaction mode the bridge default make use of it to make sure that no messages are duplicated during message transfer between transports. This enables migration from MSMQ to SqlServer without having to make changes to any of the involved endpoints.

Should `ReceiveOnly` transaction mode be prefered instead, use the following configuration:

snippet: bridge-configuration-explicit-receive-only-mode