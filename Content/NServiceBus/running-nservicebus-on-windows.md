---
title: "Running NServiceBus on Windows"
tags: ""
summary: "NServiceBus relies on a few key pieces of infrastructure in order to run properly. This page gives you an overview of the requirements for running NSerivceBus on the windows platform."
---

NServiceBus relies on a few key pieces of infrastructure in order to run properly. This page gives you an overview of the requirements for running NSerivceBus on the windows platform.

Queuing system
--------------

NServiceBus works on top of existing queuing systems to provide the reliable communications that has become the trademark of NServiceBus. By default we use Msmq which is the queuing system that comes with every installation of Windows. NServiceBus will configure MSMQ automatically for you but if you need to do it manually just make sure not to enable the Active Directory integration since that causes the addressing used be NServiceBus to not function properly. 

More on MSMQ [here.](msmq-information.md)

Distributed Transaction Coordinator
-----------------------------------

In order to support guaranteed once delivery of messages NServiceBus makes use of the Distributed Transaction Coordinator(DTC) to syncronise transaction between the queuing system and your database. For this to work correctly the MSDTC needs to be started and configured correctly. 

You can read more on transactions
[here](transactions-message-processing.md)

Storage
-------

In order to durably handle things like subscriptions, timeouts, sagas etc. NServiceBus needs a storage mechanism that supports the DTC. The default storage for NServiceBus 3 is RavenDB but relational databases is still supported through
[NHibernate](relational-persistence-using-nhibernate.md). NServiceBus will automatically install RavenDB for you if no existing installation can be detected on your machine. 

You can read more on the persistence needs of NServiceBus
[here.](persistence-in-nservicebus.md)

Performance counters
--------------------

To better help you monitoring your system NServiceBus will update a set of performance counters. In order for this to work a they need to be setup on the local machine. 

More info on the monitoring support in NServiceBus can be found
[here.](monitoring-nservicebus-endpoints.md)

Using powershell to setup the infrastructure manually
-----------------------------------------------------

Starting with NServiceBus 3.3.0 as set of powershell cmdlets are bundled with the binaries and can be used to automate setup of production servers. 

More information about the powershell support can be found
[here.](managing-nservicebus-using-powershell.md)

