---
title: NServiceBus and WebSphere/Sonic
summary: Swap out the pluggable MSMQ transport to get NServiceBus API on top of WebSphere and Sonic.
tags: []
---

{{NOTE:

* This article applies to an older version of the NServiceBus WebsphereMQ adapter. 
* To evaluate the WebsphereMQ transport for NServiceBus, See https://github.com/Particular/NServiceBus.WebSphereMQ. 
* [Contact Particular Software support](http://particular.net/ContactUs) for licensing and support details.

}}

WebSphere and Sonic are enterprise-grade middleware, robust, performant. No argument there.

But to the average .NET developer, exposed .NET API is horribly complex, looks more like Java, and does not take advantage of the strong typing provided by generics or lambdas. The developer-friendly NServiceBus API can be used on top of WebSphere and Sonic by swapping out the pluggable MSMQ transport implementation, giving you the best of both worlds. Here is an [NServiceBus adapter for WebSphere](http://code.google.com/p/nservicebuswmq/).

