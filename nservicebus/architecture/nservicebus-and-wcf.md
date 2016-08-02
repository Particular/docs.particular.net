---
title: NServiceBus and WCF
summary: "Publish/subscribe, fault-tolerance, long-running processes, interoperability"
component: Host
redirects:
 - nservicebus/nservicebus-and-wcf
---

The main thing missing from WCF is publish/subscribe, with NServiceBus it is out of the box.

The next important thing is fault-tolerance. Exceptions cause WCF proxies to break, requiring to "refresh" them in code but the call data is liable to be lost. NServiceBus provides full system rollback. Not only does the database remain consistent, but the messages return to their queues and no valuable data is lost.


## Same for plain MSMQ

Whether looking at the MSMQ bindings for WCF or programming directly against MSMQ, in both cases it is required to handle pub/sub and the transaction and exception management needed for full fault tolerance. It is also necessary to handle long-running processes with MSMQ.


## Long-running processes

WCF integrates with WF to provide a capability known as durable services. WF provides the state management facilities that hook into the communication facilities provided by WCF. Unfortunately, transaction and exception boundaries aren't specified by the infrastructure.

Unless developers are very careful about how they connect workflow activities, transaction scopes, and communications activities, the process state can be corrupted and exposed to remote services and clients. One of the reasons this is possible is that WF is designed as a generic workflow engine, not specifically for long-running processes.

Since regular business logic is simple and stable enough on its own, NServiceBus is specifically designed to handle long-running processes so they are robust and scalable by default, without developers doing anything special.

Transactions are automatically handled on a per-message basis and inherently span all communications and state-management work done by an endpoint. An exception causes all work to be undone, including the sending of any messages, so that remote services and clients do not get exposed to inconsistent data.


## Interoperability

snippet:interop