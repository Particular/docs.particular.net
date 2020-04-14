---
title: MSMQ Transport Configuration
summary: Explains the mechanics of MSMQ transport, its configuration options, and other configuration settings that were coupled to this transport
reviewed: 2020-04-14
component: MsmqTransport
tags:
 - Transport
 - MSMQ
 - Transactions
related:
 - transports/msmq/connection-strings
redirects:
 - nservicebus/msmqtransportconfig
 - nservicebus/msmq/transportconfig
---

partial: receivealgorithm

partial: disableinstaller

partial: disabledlq

partial: disableconnectioncachingforsends

partial: usenontransactionalqueues

partial: enablejournaling

partial: ttrq

partial: label


## Transactions and delivery guarantees

MSMQ Transport supports the following [Transport Transaction Modes](/transports/transactions.md):

 * Transaction scope (Distributed transaction)
 * Transport transaction - Send atomic with receive
 * Transport transaction - Receive only
 * Unreliable (Transactions disabled)

See also [Controlling Transaction Scope Options](/transports/transactions.md#controlling-transaction-scope-options).


### Transaction scope (distributed transaction)

In this mode, the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access.


partial: native-transactions


### Unreliable (transactions disabled)

In this mode, when a message is received, it is immediately removed from the input queue. If processing fails, the message is lost because the operation cannot be rolled back. Any other operation that is performed, when processing the message, is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
