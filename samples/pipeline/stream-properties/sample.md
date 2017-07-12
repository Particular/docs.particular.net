---
title: Handling large Stream properties via the pipeline
summary: Add support for writing Streams to a file share that can be accessed by multiple endpoints.
reviewed: 2016-03-21
component: Core
tags:
- Pipeline
related:
- nservicebus/pipeline
---


## Introduction

This sample leverages the pipeline to provide a pure stream based approach for sending large amounts of data. It is similar to the file share [DataBus](/nservicebus/messaging/databus/file-share.md) in that it assumes a common network file share accessible by endpoints and uses headers to correlate between a message and its connected files on disk.

The main difference is that with streams, the data doesn't all need to be loaded into memory all at once which results in a much more efficient and scalable solution.


## Stream Storage helper

This provides an extension method to `Configure` to simplify passing in settings to the stream storage.

snippet: stream-storage-helper

The helper method can then be called at configuration time.

snippet: configure-stream-storage


## Write Stream properties to disk

This happens in as part of the outgoing pipeline, see `StreamSendBehavior.cs`.

snippet: SendBehaviorDefinition

Each stream copied to disk will need a unique key.

snippet: generate-key-for-stream

Copy each stream property to disk

snippet: copy-stream-properties-to-disk

On disk (at the root of the solution for this sample) it will look like this

```
> Storage
  > 2015-03-06_15
     > 75ab3b84-8b37-4da7-bf07-b173f2f5570d
     > 92f749bc-27a8-4ba4-bc7a-b502dffe9cd9
     > 593a4670-1c09-4bbe-80ef-c22fb5356704
```

Where each GUID is a file containing the contents of the emptied stream.


## Reading back from the stream

This happens in as part of the incoming pipeline, see `StreamReceiveBehavior.cs`

snippet: ReceiveBehaviorDefinition

Copy the contents of the files on disk back into the message properties.

snippet: write-stream-properties-back

Cleanup the opened streams after message processing.

snippet: cleanup-after-nested-action


## Configuring the pipeline behaviors

snippet: pipeline-config


## The message to send

snippet: message-with-stream


## Sending with a http stream

snippet: send-message-with-http-stream


## Sending with a file stream

snippet: send-message-with-file-stream

NOTE: If using a `MemoryStream` ensure that the [Position](https://msdn.microsoft.com/en-us/library/system.io.memorystream.position.aspx) is set back to `0` before sending the message. Also note that writing large amounts of data to a `MemoryStream` will result in significant memory usage (perhaps resulting in an `OutOfMemoryException`) and put pressure on Garbage Collection.


## Handler

snippet: message-with-stream-handler


## Difference to the Databus

The [built in DataBus](/nservicebus/messaging/databus/) relies on byte arrays and memory streams to operate. As such it has limitations in the amount of data it can send.
