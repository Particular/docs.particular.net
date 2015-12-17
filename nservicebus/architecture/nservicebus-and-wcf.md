---
title: NServiceBus and WCF
summary: "Publish/subscribe, fault-tolerance, long-running processes, interoperability"
tags: []
redirects:
 - nservicebus/nservicebus-and-wcf
---

The main thing missing from WCF is publish/subscribe, but why should you have to build it yourself? With NServiceBus, you get it out of the box.

The next important thing is fault-tolerance. Exceptions cause WCF proxies to break, requiring you to "refresh" them in code but the call data is liable to be lost. NServiceBus provides full system rollback. Not only does your database remain consistent, but your messages return to their queues and no valuable data is lost.

## Same for plain MSMQ

Whether you're looking at the MSMQ bindings for WCF or programming directly against MSMQ, in both cases you have to handle pub/sub and the transaction and exception management needed for full fault tolerance. You also have to handle long-running processes with MSMQ. Here's what one CIO remarked:

> I have to say this: NServiceBus is an incredible product, I only wish there was a bit more documentation on it - we see it being used in so many places in our own software (not just queued processing, which is what we are trying to quickly patch right now) and is taking us in new directions. We were working on a WCF approach for 45 days until we threw it away and replaced it all with NServiceBus and got it working @ 99.99% in 7 business days.
Thank you for creating this great framework."
Karell Ste-Marie, CIO of BrainBank Inc.

## Long-running processes

WCF integrates with WF to provide a capability known as durable services. WF provides the state management facilities that hook into the communication facilities provided by WCF. Unfortunately, transaction and exception boundaries aren't specified by the infrastructure.

Unless developers are very careful about how they connect workflow activities, transaction scopes, and communications activities, the process state can be corrupted and exposed to remote services and clients. One of the reasons this is possible is that WF is designed as a generic workflow engine, not specifically for long-running processes.

Since regular business logic is simple and stable enough on its own, NServiceBus is specifically designed to handle long-running processes so they are robust and scalable by default, without developers doing anything special.

Transactions are automatically handled on a per-message basis and inherently span all communications and state-management work done by an endpoint. An exception causes all work to be undone, including the sending of any messages, so that remote services and clients do not get exposed to inconsistent data.

## Interoperability

You can expose your NServiceBus endpoints as WCF services with as little as one line of code and the standard WCF configuration. All you need to do is write an empty class that inherits from NServiceBus.WcfService, specifying the types of the request and the response, and NServiceBus does the rest, as follows:

    public class MyService : NServiceBus.WcfService { }

With NServiceBus, you get access to the features you need from WCF, such as interoperability, without giving up the reliability and scalability of messaging.