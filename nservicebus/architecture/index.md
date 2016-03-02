---
title: Overview
summary: NServiceBus compared to WCF and BizTalk.
tags: []
redirects:
- nservicebus/overview
---

Designed for collaboration between business-oriented services, NServiceBus is not a replacement for RPC technologies such as WCF.

Successful SOA and DDD projects use a mix of approaches and technologies not just NServiceBus for communications.

This article discusses the similarities and differences between NServiceBus and its Microsoft counterparts.


## Closer to WCF than to BizTalk

![BizTalk](biztalk.jpg)

When people hear the term "service bus", they picture a central box through which all communication goes, like BizTalk. That's actually a description of the broker architectural style, not the bus architectural style. A bus isn't necessarily a physical entity. In this respect, NServiceBus is more similar to WCF than it is to BizTalk.

There is no physical WCF one can point to in the network topology. WCF is part of the infrastructure that is run in-process with a given application's code. NServiceBus is the same.

Just like you can write your own host process and activate WCF explicitly within it, you can do the same thing with NServiceBus. The bus in NServiceBus is something virtual - a collection of framework objects running in the various application processes. You can think of it as a peer-to-peer mesh that runs alongside your code, as illustrated in the following diagram:

![deployment topology](deployment-topology.jpg)


## What's the difference?

The principles that make NServiceBus robust are decades old. Proven to hold up through countless technological shifts, the queued messaging on which NServiceBus is based is more than just an implementation choice, it's a primary architectural concept. There's no such thing as a blocking call in NServiceBus.

As a general purpose communications technology, WCF does not enforce the queued messaging paradigm. NServiceBus does, and the architectural implications are profound.

When developing systems according to the traditional RPC techniques that WCF supports, it simple and straightforward to get something working. That's when the problems start. Scalability and fault-tolerance are inherently hindered by RPC principles. At this point, it is close to impossible to solve these problems and even throwing more hardware at it has little effect. While WCF doesn't force developers down this path, it doesn't prevent them from doing so either. NServiceBus directs you away from these problems right from the beginning.


## Scalability with one-way messaging

In this presentation, Udi Dahan explains the relationship between reliability, availability, and scalability and why architects should first focus on reliability. After all, a highly available and scalable service that produces unreliable results isn't very valuable. Throughout the presentation, the value of queued messaging is underlined and the way it handles various failure scenarios is discussed.

Although the recording missed the first 5-10 minutes, the core message has not been lost.

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/6222577" webkitallowfullscreen width="400"></iframe>


## Adoption and climbing the learning curve

While it does take some getting used to, code written using NServiceBus is quite a bit simpler and shorter than before, not to mention much easier to unit test. See [Case Studies](http://particular.net/casestudies) to see NServiceBus successes stories.