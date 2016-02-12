---
title: JSON Serializer
summary: How to use the core BSON serializer.
related:
- nservicebus/serialization
- nservicebus/serialization/newtonsoft
---

This sample uses the BSON serializer in the core.

WARNING: In Version 6 of NServiceBus the built in BSON serializer has been deprecated. The [Newtonsoft serializer](/nservicebus/serialization/newtonsoft.md) can be used as a replacement
 

## Configuring to use BSON

snippet:config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet:mutator


## The message send

snippet:message