---
title: Newtonsoft Bson Serializer
summary: Using the Newtonsoft Bson serializer in an endpoint.
reviewed: 2016-09-28
component: NewtonSoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft
 - samples/encryption/newtonsoft-json-encryption
---

This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) and configures it to use BSON.


## Configuring to use NServiceBus.Newtonsoft.Json

snippet: config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet: mutator

Register the mutator.

snippet: registermutator


## The message send

snippet: message