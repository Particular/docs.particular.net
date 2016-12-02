---
title: Raw messaging using NServiceBus
summary: How to send and receive raw messages using NServiceBus transport infrastructure
component: RawMessaging
tags:
 - Raw
 - Messaging
---

NServiceBus.Raw allows sending and receiving raw messages using NServiceBus transport infrastructure. NServiceBus to messaging is like Nissan Patrol to off-roading -- a full-featured and mature tool that has all the things one might ever need. NServiceBus.Raw, on the other hand, is like an off-road buggy. It has the same Nissan Patrol super-durable axles and engine but offers no ammenities other than a chair and a steering wheel.

## Configuration

Configuration of raw endpoints is very straightforwards and follows the same patterns as regular NServiceBus endpoint configuration

snippet:Configuration

## Sending

The following code sends a message to another endpoint

snippet:Sending

## Receiving

The following code implements the on-message callback invoked when a message arrives at a raw endpoint

snippet:Receiving

Notice the method gets a `dispatcher` object which can be used to send messages. The transport transaction object can be passed from the receiving context to the dispatcher to ensure transactions spans both send and receive if the underlying transport infrastructure supports such mode.