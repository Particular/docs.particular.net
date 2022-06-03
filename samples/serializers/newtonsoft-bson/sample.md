---
title: Newtonsoft BSON Serializer
summary: Using the Newtonsoft BSON serializer in an endpoint
reviewed: 2022-06-03
component: NewtonSoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft
redirects:
 - samples/serializers/bson
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
