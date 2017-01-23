---
title: ServiceControl, MSDTC and transaction support
summary: Summary of ServiceControl messaging reliability
tags:
- ServiceControl
- MSDTC
---

ServiceControl does not rely on the Microsoft Distributed Transaction Coordinator (MSDTC) for exact-one processing of its messages. ServiceControl can be used in environments that do not rely on MSDTC.


## Receiving of messages

All handlers within ServiceControl handle messages idempotently. Messages can be processed multiple times successfully without corrupting ServiceControl data.


## Sending of messages

### Retries

ServiceControl can be used to retry messages that have been send to the error queue. When a message is retried it is moved to an internally owned staging queue and then forwarded back to the processing endpoint from there.

If ServiceControl is running on a transport that supports [Sends atomic with Receive](/nservicebus/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transactions then the message is guaranteed to only be sent once. ServiceControl is able to prevent multiple outstanding retries for the same message at once. 

If ServiceControl is running on a transport that does not support this type of transaction then the message may be delivered to the processing endpoint multiple times. This is the expected behavior for these transports.  

### Integration Events

ServiceControl can also publish [external integration events](/servicecontrol/contracts.md). If ServiceControl crashes while sending these messages it is possible that these events can get deleivered to subscribers more than once. 