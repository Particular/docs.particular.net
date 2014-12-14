---
title: Using Express Messages
summary: When and how to use Express Messages
tags: []
---

##MSMQ Transport

MSMQ is based on the concept of Store and Forward, where messages are stored durably on disk at the sender and then delivered by MSMQ to the receiver. This behavior can be changed using Express Messages, but this would make the message vulnerable to server crashes and restarts. For more information on express messages when using MSMQ: http://msdn.microsoft.com/en-us/library/ms704130

To use express messages, decorate messages with the `[Express]` attribute:

## Using an Attribute

<!-- import ExpressMessageAttribute -->

## Using conventions

<!-- import ExpressMessageConvention -->

