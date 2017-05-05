---
title: Learning Transport
component: LearningTransport
reviewed: 2017-05-01
related:
 - samples/learning-transport
---

The Learning Transport simulates queuing infrastructure by storing all message actions in the local file system. All files and directories are relative to the current solution directory.

include: learning-transport-warning

Added in Version 6.3.

Some recommended use cases are:

 * Learning/Experimenting with NServiceBus features.
 * Building a [spike](https://en.wikipedia.org/wiki/Spike_(software_development)) or [demo](https://simple.wikipedia.org/wiki/Demo_(software)).
 * Reproducing a bug that is not related to a production transport, for example when raising a support case.

Currently [ServiceControl](/servicecontrol/) (and hence [ServicePulse](/servicepulse/) and [ServiceInsight](/serviceinsight/)) are not supported.


## Usage

snippet: LearningTransport


## File System Structure

All required information is stored in a `.learningtransport` directory that exists at the solution root.

WARNING: When using source control the `.learningtransport` directory should be excluded and never committed.


### Message File Types


#### Body File

The serialized contents of a message.

 * Serialization is done using the configured [Serializer](/nservicebus/serialization/).
 * File convention is `[MessageId].body.txt`.


#### Metadata File

A serialized representation of a messages metadata.

 * First line is the path to the Message Body File.
 * Remaining lines are the json serialized headers of the message.
 * File convention is `[MessageId].metadata.txt`.


### Error Directory

When a message fails processing both its metadata and body files will be moved to the "error" directory. The name of the directory will be derived from [configured error queue address](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address). The default error queue name, and hence directory name, is "error".


### Endpoint Structure

Each endpoint will have a corresponding directory under `.learningtransport` with the following structure


#### Metadata files

Each incoming messages will have a corresponding Metadata File at the root of the endpoint directory.


#### .bodies (directory)

Each incoming messages will have a corresponding Body in this directory.


#### .delayed (directory)

Used to store messages that have been send with a [Delayed Delivery](/nservicebus/messaging/delayed-delivery.md). One directory per time stamp (of a second granularity) with the format `yyyyMMddHHmmss`. Each timestamp directory can contain multiple Metadata Files.


#### .pending (directory)

Transaction directory used to mark a message as being processed. Also prevents duplicate processing.


#### .committed (directory)

Used to temporarily store outgoing message that are sent during the processing of another message.
