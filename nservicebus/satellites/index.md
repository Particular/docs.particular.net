---
title: Satellites
summary: Extension point for raw processing of messages.
reviewed: 2016-08-01
component: Core
versions: '[5,]'
related:
 - nservicebus/pipeline/manipulate-with-behaviors
 - nservicebus/pipeline/features
 - nservicebus/recoverability
redirects:
 - nservicebus/pipeline/satellites
---

Satellites are light weight message processors that runs in the same process as its owning endpoint. While are mostly used by NServiceBus to implement infrastructure features like the TimeoutManager and the Gateway they can be used in scenarios where messages from additional queues other than the main input queue needs to be processed. This is useful when the robustness of a separate queue is needed without having to create and setup a new endpoint and configure any message mappings.


partial: content