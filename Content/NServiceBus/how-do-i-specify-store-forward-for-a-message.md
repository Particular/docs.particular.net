---
title: How to disable Store and Forward for a Message?
summary: Store and Forward is not always the preferred method.
tags: []
---

MSMQ is based on the concept of Store and Forward, where messages are stored locally at the sender and then delivered by MSMQ to the receiver. This behaviour can be changed, but this makes the message vulnerable to server crashes and restarts.

To disable the Store and Forward behaviour, decorate messages with the `[Express]` attribute:

## Using an Attribute

<!-- import ExpressMessageAttributeV4 -->

## Using conventions

### Version 4

<!-- import ExpressMessageConventionV4 -->

### Version 5

<!-- import ExpressMessageConventionV5 -->
