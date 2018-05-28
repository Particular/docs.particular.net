---
title: Newtonsoft Bson Serializer
summary: Using the Newtonsoft Bson serializer in an endpoint
reviewed: 2018-05-23
component: NewtonSoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft
 - samples/encryption/newtonsoft-json-encryption
---

This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) and configures it to use [BSON](http://bsonspec.org/).


## Configuring to use NServiceBus.Newtonsoft.Json

snippet: config


## Diagnostic mutator

A helper that will write out the contents of any incoming message.

snippet: mutator

Register the mutator.

snippet: registermutator


## Sending a message

snippet: message