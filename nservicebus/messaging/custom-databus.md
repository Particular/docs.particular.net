---
title: Custom DataBus implementation
summary: Details how to register and plugin custom DataBus implementation into an endpoint.
tags:
- DataBus
redirects:
- nservicebus/plugin-custom-databus
---

NServiceBus endpoints support sending and receiving large chunks of data via the [DataBus feature](databus.md).

It is possible to create a custom DataBus implementation by implementing the `IDataBus` interface, such as in the following minimalistic sample:

snippet:CustomDataBus

To configure the endpoint to use the custom DataBus implementation it is enough to register it at endpoint configuration time, such as in the following sample:

snippet:PluginCustomDataBus