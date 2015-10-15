---
title: WCF Integration
summary: Illustrates how to map between WCF and messages on the bus.
tags:
related:
---


## Introduction




## WCF Helpers

<!-- import WcfMapper -->

<!-- import ClientChannelBuilder -->

<!-- import CallbackService -->


## Receiving Endpoint Configuration

<!-- import startbus -->

<!-- import startwcf -->


## Client Configuration

<!-- import Send -->

<!-- import SendHelper -->

Note that, for the purposes of this sample, for every call it creates a new `ChannelFactory` and `ICommunicationObject`. Depending on your specific use case you mat want to apply different scoping, lifetime and cleanup rules for these instances.
