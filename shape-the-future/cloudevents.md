---
title: NServiceBus and CloudEvents
summary: Using NServiceBus with CloudEvents
reviewed: 2024-09-16
---

![](cloudevents-horizontal-color.png)

[CloudEvents](https://cloudevents.io) is a [specification](https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md) for describing event data in a common way. CloudEvents seeks to dramatically simplify event declaration and delivery across services, platforms, and beyond.

Starting NServiceBus V10.1, endpoints can receive messages wrapped in a CloudEvents envelope via the [NService.Envelope.CloudEvents](https://github.com/Particular/NServiceBus.Envelope.CloudEvents/) package. For more information on configuring endpoints to handle CloudEvents, refer to the [CloudEvents documentation](/nservicebus/cloudevents.md).

If you are interested in NServiceBus endpoints being able to send messages using the CloudEvents specification, [tell us more about your CloudEvents use cases](https://github.com/Particular/NServiceBus/issues/7159).
