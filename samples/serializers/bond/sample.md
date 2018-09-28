---
title: Bond Serializer Usage
summary: Using the Bond serializer in an endpoint
component: Bond
reviewed: 2018-09-28
related:
 - nservicebus/serialization
 - nservicebus/serialization/bond
---


## Configuring NServiceBus to use Bond

snippet: config


## The message class

snippet: message


## Sending a message

The message is decorated with [Bond attributes](https://microsoft.github.io/bond/manual/bond_cs.html#attributes).

snippet: messagesend

Note that for simplicity, this sample does not use [Bond code generation](https://microsoft.github.io/bond/manual/bond_cs.html#code-generation) or the [Bond.Compiler.CSharp/Bond.CSharp](https://microsoft.github.io/bond/manual/bond_cs.html#nuget-packages) for schema to code generation.