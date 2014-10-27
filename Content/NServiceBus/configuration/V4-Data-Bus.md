---
title: Configuration API Data Bus in V3 and V4
summary: Configuration API Data Bus in V3 and V4
tags:
- NServiceBus
- BusConfiguration
- V3
- V4
---

Large chunks of data such as images or video files can be transported using NServiceBus DataBus. The DataBus is useful to overcome transports limitations that generally impose a maximum size on the transported message.

To configure the DataBus feature, call the `FileShareDataBus( string pathToSharedFolder )`,  of the `Configure` instance, passing as argument a path that must be accessible by all the endpoints that need to share the same messages.

* [DataBus / Attachments](/nservicebus/attachments-databus-sample).