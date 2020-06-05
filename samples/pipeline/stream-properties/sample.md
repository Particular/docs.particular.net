---
title: Handling Stream Properties Via the Pipeline
summary: Add support for writing streams to a file share that can be accessed by multiple endpoints.
reviewed: 2019-12-11
component: Core
related:
- nservicebus/pipeline
---


This sample leverages the [message handling pipeline](/nservicebus/pipeline/) to provide a pure stream-based approach for sending large amounts of data. It is similar to the file share [data bus](/nservicebus/messaging/databus/file-share.md) in that it assumes a common network file share accessible by endpoints and uses headers to correlate between a message and its connected files on disk.

The main difference is that with streams, the data doesn't need to be loaded into memory all at once which results in a more efficient and scalable solution.


## Stream storage helper

This provides an extension method to simplify passing in settings to the stream storage.

snippet: stream-storage-helper

The helper method can then be called at configuration time.

snippet: configure-stream-storage


## Write stream properties to disk

This happens as part of the outgoing pipeline, see `StreamSendBehavior.cs`.

snippet: SendBehaviorDefinition

Each stream copied to disk will need a unique key.

snippet: generate-key-for-stream

Copy each stream property to disk

snippet: copy-stream-properties-to-disk

The file streams will appear on disk at the root of the solution in a folder called `storage`. Here is a sample file structure:

```
> storage
  > 2015-03-06_15
     > 75ab3b84-8b37-4da7-bf07-b173f2f5570d
     > 92f749bc-27a8-4ba4-bc7a-b502dffe9cd9
     > 593a4670-1c09-4bbe-80ef-c22fb5356704
```

Each GUID is a file containing the contents of the emptied stream.


## Reading back from the stream

This happens in as part of the incoming pipeline, see `StreamReceiveBehavior.cs`

snippet: ReceiveBehaviorDefinition

Copy the contents of the files on disk back into the message properties.

snippet: write-stream-properties-back

Clean up the opened streams after message processing.

snippet: cleanup-after-nested-action


## Configuring the pipeline behaviors

snippet: pipeline-config


## The message to send

snippet: message-with-stream


## Sending with an HTTP stream

snippet: send-message-with-http-stream


## Sending with a file stream

snippet: send-message-with-file-stream

NOTE: When using a `MemoryStream` ensure that the [Position](https://msdn.microsoft.com/en-us/library/system.io.memorystream.position.aspx) is set back to `0` before sending the message. Also note that writing large amounts of data to a `MemoryStream` will result in significant memory usage (perhaps resulting in an `OutOfMemoryException`) and put pressure on the garbage collector.


## Handler

snippet: message-with-stream-handler


## Difference to the databus

The [built-in databus](/nservicebus/messaging/databus/) relies on byte arrays and memory streams to operate. As such, there are limits to the amount of data that it can send.
