---
title: Bond Serializer Usage
summary: Using the Bond serializer in an endpoint.
component: Bond
reviewed: 2016-11-07
related:
 - nservicebus/serialization
 - nservicebus/serialization/bond
---


## Configuring to use Bond

snippet: config


## The message

snippet: message


## The message send

The message is decorated with  [Bond Attributes](https://microsoft.github.io/bond/manual/bond_cs.html#attributes).

snippet: messagesend

Note that, for simplicity, this sample does not use [Bond Code generation](https://microsoft.github.io/bond/manual/bond_cs.html#code-generation) or the [Bond.Compiler.CSharp/Bond.CSharp](https://microsoft.github.io/bond/manual/bond_cs.html#nuget-packages) for schema to code generation.