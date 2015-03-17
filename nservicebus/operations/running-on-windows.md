---
title: Running on Windows
summary: Overview of the requirements for running on windows (including MSMQ, MSDTC, Storage and powershell)
tags:
- MSMQ
- MSDTC
- Performance Counters
- Powershell
- Storage
- NHibernate
redirects:
- nservicebus/running-nservicebus-on-windows
---

NServiceBus relies on a few key pieces of infrastructure in order to run properly. This page gives you an overview of the requirements for running NServiceBus on the windows platform.

## Queuing system

NServiceBus works on top of existing queuing systems to provide the reliable communications that has become the trademark of NServiceBus. By default we use MSMQ which is the queuing system that comes with every installation of Windows. Configuring NServiceBus via the [PlatformInstaller](http://particular.net/downloads) will configure MSMQ automatically for you but if you need to do it manually just make sure not to enable the following components:
- MSMQ Active Directory Domain Services Integration
- MSMQ Http Support
- MSMQ Triggers
- Multicasting Support
- MSMQ DCOM Proxy
as they cause the addressing used in NServiceBus to not function properly 

To read more about MSMQ go [here](/nservicebus/msmq/).

## Distributed Transaction Coordinator

In order to support guaranteed once delivery of messages NServiceBus makes use of the Distributed Transaction Coordinator(DTC) to synchronise transaction between the queuing system and your database. For this to work correctly the MSDTC needs to be started and configured correctly. 

You can read more on transactions
[here](/nservicebus/operations/transactions-message-processing.md)

Since version 5 of NServiceBus there is a _non-DTC_ mode of operation available. In this mode NServiceBus uses a concept of outbox, a message store backed by same DB as the user code, to temporarly store messages that need to be send as a result of processing an incoming message. To read more about this subject see [Outbox](/nservicebus/outbox/).

## Storage

In order to durably handle things like subscriptions, timeouts, sagas, etc, NServiceBus needs a storage mechanism that supports the MSDTC (or when in _non-DTC_ mode, uses the same DB as the user code). The default storage for NServiceBus 3 and 4 was RavenDB with an option of using relational databases via [NHibernate](/nservicebus/nhibernate/). Since version 5 there is no default storage and a user has to explicitly choose either RavenDB, NHibernate or non durable. 

You can read more on the persistence needs of NServiceBus [here](/nservicebus/persistence/).

## Performance counters

To better help you monitoring your system NServiceBus will update a set of performance counters. In order for this to work a they need to be setup on the local machine. 

More info on the monitoring support in NServiceBus can be found [here](/nservicebus/operations/monitoring-endpoints.md).

## Using powershell to setup the infrastructure manually

Starting with NServiceBus 3.3.0 as set of powershell cmdlets are bundled with the binaries and can be used to automate setup of production servers. 

More information about the powershell support can be found [here](management-using-powershell.md).

