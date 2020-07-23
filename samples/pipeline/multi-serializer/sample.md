---
title: Control of serialization via the pipeline
summary: Support for attribute-based message serialization
reviewed: 2020-07-23
component: Core
related:
- nservicebus/pipeline
- nservicebus/serialization
---

NOTE: A subset of the functionality described in this sample was made a part of NServiceBus version 6. See [Serialization in NServiceBus](/nservicebus/serialization/) for more information.


## Introduction

This sample leverages the pipeline to provide an attribute-based message serialization feature. It is currently hard-coded to support only XML and JSON serialization. It uses attributes, defined at the message level, to switch messages between different serializations, but any code could be substituted here to control the choice of serialization.

WARNING: This sample is not compatible with message serialization against NServiceBus version 4 and below. For simplicity of the sample, some wire compatibility workarounds are excluded. Review the current serialization behaviors in the core of NServiceBus for more details.


## Code walk-through

The solution contains 3 projects

 * `Shared` contains the common message declarations and the behavior functionality
 * `Sender` and `Receiver` are the endpoints using the behaviors


### The attribute definitions

These can be used to decorate messages.

snippet: attributes


### The message definitions

The messages use the above attributes to control how they are serialized.

snippet: message-definitions


### Serialization mapper

This class interrogates the message information and uses that to determine which serializer to use.

snippet: serialization-mapper


### Behavior configuration

This replaces the existing serialization behavior and also add the serialization mapper to [dependency injection](/nservicebus/dependency-injection/).

snippet: behavior-configuration


### Behaviors

`DeserializeBehavior.cs` and `SerializeBehavior.cs` are mostly copies of the core NServiceBus behaviors. The difference is that instead of using the core default serializer, they request a serializer from the serialization mapper.


#### Serialization behavior

snippet: serialize-behavior


#### Deserialization behavior

snippet: deserialize-behavior


## Running the code

### A simple execution

 * Set both `Receiver` and `Sender` as startup projects
 * Run the solution
 * In `Sender` press <kbd>J</kbd> (for a JSON message) and <kbd>X</kbd> (for an XML message)
 * The message will be received by `Receiver`


### The message on the wire

 * Start only the `Sender`
 * Send both JSON and SML

Now look at the [learning transport's message storage](/transports/learning/viewing-messages.md) and there will be two messages in the `Receiver` folder.

**An XML message with the following content**

```xml
<?xml version="1.0"?>
<MessageWithXml xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.net/">
<SomeProperty>Some content in an XML message</SomeProperty>
</MessageWithXml>
```

**And a JSON message with the following content**

```js
{"SomeProperty":"Some content in a JSON message"}
```