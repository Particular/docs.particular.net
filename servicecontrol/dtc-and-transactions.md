---
title: ServiceControl, MSDTC and transaction support
summary: ServiceControl uses idempotent message processing and does not rely on MSDTC for exact-once processing but does use transport transactions is supported.
tags:
- ServiceControl
- MSDTC
---

ServiceControl does not rely on the Microsoft Distributed Transaction Coordinator (MSDTC) for exact-one processing of its messages. Meaning that ServiceControl can be used in environments that do not rely on MSDTC

## Receiving of messages

It processes its messages idempotently when storing messages in its database. Duplicate messages will not result in any issues.

## Sending of messages

ServiceControl can be used to retry messages but also publishes several events. These messages are send as atomic as possible. We have potentially two transactions, the transport transaction and the persistence (RavenDB) transaction. 

Tranports that *do not* support transactions will result in potentially sending the same retry more then once. This should not be any issue as this is expected behavior for this transport.

Transports that *do* support transactions can potentially result in not delivering the retry as the database transaction will be committted before the transport transaction.
