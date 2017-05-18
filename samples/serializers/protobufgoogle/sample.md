---
title: ProtocolBuf-Google Serializer Usage
summary: Using the ProtoBuf-Google serializer in an endpoint.
reviewed: 2017-05-17
component: ProtoBufGoogle
related:
- nservicebus/serialization
---

## Configuring to use ProtoBuf

snippet: config


## The message send

snippet: messagesend


## The message definition

The sample uses a `.proto` file to generate the message contract.

snippet: proto

This is done using `proroc.exe`

```dos
protoc.exe -I=C:\Code\Sample --csharp_out=C:\Code\Sample C:\Code\Sample\CreateOrder.proto
```

With the resultant class definitions residing in `CreateOrder.cs`

To mark the generated class as an NServiceBus `IMessage` a partial class is used.

snippet: partial

include: protobufgoogleinfo

