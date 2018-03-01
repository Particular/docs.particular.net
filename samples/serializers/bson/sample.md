---
title: BSON Serializer
summary: Using the core BSON serializer
reviewed: 2018-03-01
component: Json
related:
- nservicebus/serialization
- nservicebus/serialization/newtonsoft
---

This sample uses the BSON serializer in the core.

WARNING: In NServiceBus version 6 and above the built-in BSON serializer has been deprecated. The [Newtonsoft serializer](/nservicebus/serialization/newtonsoft.md) can be used as a replacement


## Configuring to use BSON

snippet: config


## Diagnostic mutator

A helper that will log the contents of any incoming message.

snippet: mutator


## The message send

snippet: message
