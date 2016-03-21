---
title: Newtonsoft Bson Serializer
summary: Using the Newtonsoft Bson serializer in an endpoint.
reviewed: 2016-03-21
related:
- nservicebus/serialization
---

## NServiceBus.Newtonsoft.Json

This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) to provide full access to the [Newtonsoft Json.net](http://www.newtonsoft.com/json) API.


## Configuring to use NServiceBus.Newtonsoft.Json

snippet:config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet:mutator


## The message send

snippet:message