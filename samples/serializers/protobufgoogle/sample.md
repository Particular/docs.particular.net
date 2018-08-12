---
title: ProtoBuf-Google Serializer Usage
summary: Using the ProtoBuf-Google serializer in an endpoint.
reviewed: 2017-05-17
component: ProtoBufGoogle
related:
- nservicebus/serialization
---

## Configuring an endpoint to use ProtoBuf

snippet: config


## Sending a message

snippet: messagesend


## The message definition

The sample uses a `.proto` file to generate the message contract.

snippet: proto

This is done using `protoc.exe`

```dos
protoc.exe -I=C:\Code\Sample --csharp_out=C:\Code\Sample C:\Code\Sample\CreateOrder.proto
```

With the resultant class definitions residing in `CreateOrder.cs`

Here, a partial class is used to identify the generated class as an NServiceBus message by inheriting from the `IMessage` interface.

snippet: partial

include: protobufgoogleinfo

