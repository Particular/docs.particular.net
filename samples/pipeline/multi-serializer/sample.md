---
title: Control of serialization via the pipeline
summary: Add support for attribute based message serialization.
reviewed: 2018-01-26
component: Core
tags:
- Pipeline
- Serialization
related:
- nservicebus/pipeline
- nservicebus/serialization
---

NOTE: A subset of the functionality described in this sample was made a part of NServiceBus Version 6. See [Serialization in NServiceBus](/nservicebus/serialization/) for more information.


## Introduction

This sample leverages the pipeline to provide an attribute based message serialization feature.

It is currently hard coded to only support xml and json serialization. It uses attributes, defined at the message level, to switch messages between different serializations, but any code could be substituted here to control the choice of serialization.

WARNING: This sample is not compatible with message serialization against Version 4 and below of NServiceBus. The reason is that, for simplicity of the sample, some wire compatibility workarounds are excluded. Have a look at the current serialization behaviors in the core of NServiceBus for more details.


## Code Walk Through

The solution contains 3 projects

 * `Shared` contains the common message declarations and the behavior functionality.
 * `Sender` and `Receiver` are the endpoints using the behaviors.


### The attribute definitions

These can be used to decorate messages.

snippet: attributes


### The message definitions

The messages use of the above attributes to control how they are serialized.

snippet: message-definitions


### Serialization Mapper

This class interrogates the message information and derives what serializer to use.

snippet: serialization-mapper


### Behavior Configuration

This replaces the existing serialization behavior and also add the Serialization Mapper to [dependency injection](/nservicebus/dependency-injection/).

snippet: behavior-configuration


### Behaviors

`DeserializeBehavior.cs` and `SerializeBehavior.cs` are mostly copies of the core NServiceBus behaviors. The main difference is that instead of using the core default serializer they request a serializer from Serialization Mapper.


#### Serialization Behavior

snippet: serialize-behavior


#### Deserialization Behavior

snippet: deserialize-behavior


## Running the Code


### A simple execution

 * Set both `Receiver` and `Sender` as startup projects.
 * Run the solution.
 * In `Sender` press `J` (for a json message) and `X` (for a xml message).
 * The message will be received at `Receiver`.


### The message on the wire

 * Start only the `Sender`.
 * Send both json and xml.

Now have a look at the [Learning Transport's message storage](/transports/learning/viewing-messages.md) and there will be two messages in the `Receiver` folder.

**A xml message with the content**

```xml
<?xml version="1.0"?>
<MessageWithXml xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.net/">
<SomeProperty>Some content in a Xml message</SomeProperty>
</MessageWithXml>
```

**And a Json message with the content**

```js
{"SomeProperty":"Some content in a json message"}
```
