---
title: Learning Transport
component: LearningTransport
reviewed: 2017-05-01
related:
 - samples/learning-transport
 - persistence/learning
redirects:
 - nservicebus/learning-transport
---

The Learning Transport simulates queuing infrastructure by storing all message actions in the local file system. All files and directories are relative to the current solution directory.

include: learning-warning

Added in Version 6.3.

include: learning-usages

Currently [ServiceControl](/servicecontrol/) (and hence [ServicePulse](/servicepulse/) and [ServiceInsight](/serviceinsight/)) are not supported.

### Publish and subscribe

The learning transport simulates a [multicast transport](/transports/types.md#multicast-enabled-transports) this means that routing configuration isn't needed in order to publish events. 

See the [native pubsub](/nservicebus/messaging/publish-subscribe/#mechanics-native) documentation for further details.

## Usage

snippet: LearningTransport

### Transactions and delivery guarantees

The transport supports the following [Transport Transaction Modes](/transports/transactions.md):

 * Sends atomic with Receive (Default)
 * Receive Only
 * Unreliable (Transactions Disabled)

### Concurrency

By default the transport runs with concurrency limited to 1. See the [tuning](/nservicebus/operations/tuning.md) for details on how to configure concurrency levels.

Note: Production transports will run with higher concurrency setting by default

### Storage Directory

By default all data is stored in a `.learningtransport` directory that exists at the solution root.

To configure the storage location:

snippet: StorageDirectory

WARNING: When using source control the storage directory should be excluded and never committed.


### Payload size restriction

To simulate a real transport, the serialized message size supported by the learning transport is limited to 64 kB. To remove this restriction:

snippet: NoPayloadSizeRestriction


## File System Structure

### Subscription metadata

Native publish and subscribe is simulated by storing subscriber metadata in a `.events` folder in the root storage directory. Subscribers will write their address to a file named `{storage directory}\.events\{message type fullname}\{endpointname}.subscription`. Publishers will send copies of each published event to all registered subscribers.

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