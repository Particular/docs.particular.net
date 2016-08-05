---
title: Bus versus broker architecture
summary: Using NServiceBus for communication between services in SOA and DDD projects. Bus versus broker architectural styles.
reviewed: 2016-08-05
redirects:
- nservicebus/overview
---

NServiceBus is designed for communication between business-oriented services in SOA and DDD projects. It is not a replacement for RPC technologies such as WCF. Successful systems use a mix of approaches and technologies for communication, not just NServiceBus.

It takes some time to get used to the new approach, but the code written using NServiceBus is simple, more concise and easy to unit test. The systems using NServiceBus are reliable and scalable.


This article discusses the similarities and differences between NServiceBus and its Microsoft counterparts.


## Bus versus broker architectural styles 

A "service bus" is often illustrated as a central box, through which all communication goes. Despite the common understanding, that's actually a description of the **_broker architectural style_**. 

A good example is BizTalk:

![BizTalk](biztalk.jpg)


A **_bus_** in the context of the **_bus architectural style_**, isn't necessarily a physical entity. There's no physical _bus_ one can point to in the network topology. The _bus_ is part of the infrastructure that is run in-process with a given application's code. It's like a peer-to-peer mesh that runs alongside code.

A good example is WCF:

![deployment topology](deployment-topology.jpg)

NServiceBus is similar to WCF more than BizTalk. Just like it is possible write a host process and activate WCF explicitly within it, the same can be done with NServiceBus. 


## Messaging versus RPC

NServiceBus enforces queued messaging, which has profound architectural implications. The principles and patterns underlying queued messaging are decades old and battle-tested through countless technological shifts.

It's very simple and straightforward to build an application and get it working using traditional RPC techniques that WCF supports. However, scalability and fault-tolerance are inherently hindered. When using blocking calls it's close to impossible to solve these problems, and even throwing more hardware at it has little effect. WCF doesn't force developers down this path, but it also doesn't prevent them.

NServiceBus directs away from these problems right from the beginning. There's no such thing as a blocking call when one uses one-way messaging. Common, transient errors can be resolved automatically, it's also very easy to recover from failures that require some manual intervention. Above all, even when something goes wrong, no data gets lost. 

In order to learn more about the relationship between messaging and reliable, scalable, highly-available systems, watch Udi Dahan's presentation:

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/6222577" webkitallowfullscreen width="400"></iframe>

Refer to the [Architectural Principles](/nservicebus/architecture/principles.md) article to learn more about SOA and how NServiceBus aligns with SOA.

See [Case Studies](http://particular.net/casestudies) to learn how NServiceBus is used and read the successes stories.