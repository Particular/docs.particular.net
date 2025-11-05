---
title: Newtonsoft BSON Serializer
summary: Using the Newtonsoft BSON serializer in an endpoint
reviewed: 2025-11-05
component: NewtonSoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft
redirects:
 - samples/serializers/bson
---

This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) and configures it to use [BSON](http://bsonspec.org/).

## Configuration

snippet: config

## Diagnostic mutator

This helper will write out the contents of any incoming message so that the serialization can be observed.

snippet: mutator

This registers the mutator:

snippet: registermutator

## Sending a message

snippet: message
