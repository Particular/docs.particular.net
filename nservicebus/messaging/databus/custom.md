---
title: Custom DataBus implementation
summary: Details how to register and plugin custom DataBus implementation into an endpoint.
component: Core
reviewed: 2017-08-18
tags:
- DataBus
redirects:
- nservicebus/plugin-custom-databus
---

Endpoints support sending and receiving large chunks of data via the [DataBus](./).

partial: databus_content

When customizing DataBus, it may be desirable to also [customize the DataBus serializer](/samples/databus/custom-serializer/) to use Json instead of binary serialization.