---
title: Running on Windows
summary: Overview of the requirements for running on windows (including MSMQ, DTC, persistence and PowerShell).
reviewed: 2017-04-25
tags:
- MSMQ
- DTC
- PowerShell
redirects:
- nservicebus/running-nservicebus-on-windows
related:
 - nservicebus/operations
 - nservicebus/outbox
 - transports/msmq
 - persistence
---

NServiceBus relies on a few key pieces of infrastructure in order to run properly. This page gives an overview of the requirements for running NServiceBus on the Windows platform.


## Queuing system

NServiceBus works on top of existing queuing systems to provide the reliable communications that has become the trademark of NServiceBus. By default [MSMQ](/transports/msmq/) is used which is the queuing system that comes with every installation of Windows. Configuring NServiceBus via the [PlatformInstaller](https://particular.net/downloads) will configure MSMQ automatically. But when installing manually **do not** enable the following components:

 * MSMQ Active Directory Domain Services Integration
 * MSMQ Http Support
 * MSMQ Triggers
 * Multicasting Support
 * MSMQ DCOM Proxy

These components can cause issues with the addressing used in NServiceBus.


## Distributed Transaction Coordinator

In order to support guaranteed [once delivery of messages](/nservicebus/operations/transactions-message-processing.md) NServiceBus makes use of the Distributed Transaction Coordinator (DTC) to synchronize transactions between the queuing system and the database. For this to work correctly the DTC must be started and configured correctly.

In Versions 5 and above of NServiceBus there is a _non-DTC_ mode of operation available. In this mode NServiceBus uses a concept of outbox, a message store backed by same DB as the user code, to temporarily store messages that need to be sent as a result of processing an incoming message. To read more about this subject see [Outbox](/nservicebus/outbox/).


## [Persistence](/persistence/)

In order to durably handle things like subscriptions, timeouts, and sagas, NServiceBus needs a storage mechanism that supports the DTC (or when in _non-DTC_ mode, uses the same DB as the user code). The default storage for NServiceBus 3 and 4 was RavenDB with an option of using relational databases via [NHibernate](/persistence/nhibernate/). Since Version 5 there is no default storage and a user has to explicitly choose either RavenDB, NHibernate or non-durable.


## Using PowerShell to setup the infrastructure manually

NServiceBus 3.3.0 introduced a set of [PowerShell commandlets](management-using-powershell.md) that were bundled with the binaries and could be used to automate setup of production servers.

These PowerShell commandlets are now available as a standalone installation.
