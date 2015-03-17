---
title: Handling large Stream properties via the pipeline 
summary: Add support for writing Streams to a file share that can be access by multiple endpoints
tags:
- Pipeline
- Stream
- Large messages
---

## Introduction

This sample leverages the pipeline to provide an attribute based message serialization feature. 

It is currently hard coded to only support binary and json serialization. It uses attributes, defined at the message level, to switch messages between different serializations but any code could be substituted here to control the choice of serialization.

WARNING: This sample is not compatible with message serialization against Version 3 or Version 4 of NServiceBus. The reason is that, for simplicity of the sample, some wire compatibility works around are excluded. 

## Code Walk Through

The solution contains 3 projects

 * `Shared` contains the common message declarations and the actual behavior functionality.
 * `Sender` and `Receiver` are the actual endpoint using the behaviors.

### The attribute definitions 

These can be used to decorate messages. 

<!-- import attributes -->

### The message definitions

The messages use of the above attributes to control how they are serialized.

<!-- import message-definitions -->

### Serialization Mapper

This class interrogates the message information and derives what serializer to use.

<!-- import serialization-mapper -->

### Behavior Configuration

This replaces the existing serialization behavior and also injects the Serialization Mapper into the container.

<!-- import behavior-configuration -->

### Behaviors

`DeserializeBehavior.cs` and `SerializeBehavior.cs` are mostly a copies of the core NServiceBus behaviors. The main difference is that instead of using the core default serializer they request a serializer from Serialization Mapper.

#### Serialization Behavior

<!-- import serialize-behavior -->

#### Deserialization Behavior

<!-- import deserialize-behavor -->

## Running the Code

### A simple execution

 * Set both `Receiver` and `Sender` as startup projects.
 * Run the solution.
 * In `Sender` press `J` (for a json message) and `B` (for a binary message).
 * The message will be received at `Receiver`.

### The message on the wire

 * Start only the `Sender`.
 * Send both json and binary. 

Now have a look in msmq and there will be two messages in the 
`Receiver` queue.

**A binary message with the content** 

```
System.Collections.Generic.List`1[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
_items _size _version=Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
MessageWithBinary
<SomeProperty>k__BackingField           Some content in a binary message 
```

**And a Json message with the content**

```
{"SomeProperty":"Some content in a json message"}
```