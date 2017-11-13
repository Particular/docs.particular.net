---
title: MSMQ Transport Configuration
summary: Explains the mechanics of MSMQ transport, its configuration options and various other configuration settings that were at some point coupled to this transport
reviewed: 2016-10-17
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

partial: intro

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
 * Transport transaction - Sends atomic with Receive
 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)

See also [Controlling Transaction Scope Options](/transports/transactions.md#controlling-transaction-scope-options).


### Transaction scope (Distributed transaction)

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access.


partial: native-transactions


### Unreliable (Transactions Disabled)

In this mode, when a message is received, it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed, when processing the message, is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
