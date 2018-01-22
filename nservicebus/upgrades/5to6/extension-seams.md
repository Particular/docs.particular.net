---
title: Extension Seam changes in Version 6
reviewed: 2016-10-26
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## [Timeout](/nservicebus/sagas/timeouts.md) storage

`IPersistTimeouts` has been split into two interfaces, `IPersistTimeouts` and `IQueryTimeouts`, to properly separate those storage concerns. Both must be implemented to have a fully functional timeout infrastructure.

`IQueryTimeouts` implements the concern of polling for timeouts outside the context of a message pipeline. `IPersistTimeouts` implements the concern of storage and removal for timeouts which is executed inside the context of a pipeline. Depending on the design of the timeout persisters, those concerns can now be implemented independently. Furthermore, `IPersistTimeouts` introduced a new parameter `TimeoutPersistenceOptions `. This parameter allows access to the pipeline context. This enables timeout persisters to manipulate everything that exists in the context during message pipeline execution.


## [Outbox](/nservicebus/outbox/)

`IOutboxStorage` introduced a new parameter `OutboxStorageOptions`. This parameter gives access to the pipeline context. This enables outbox storage methods to manipulate everything that exists in the context during message pipeline execution.


## [Queue creation](/transports/queuecreation.md)

In Version 5 the implementation of the interface `ICreateQueues` was called for each queue that needed to be created. In Version 6 `ICreateQueues` has been redesigned. The implementation of the interface gets called once but with all queues provided on the `QueueBindings` object. It is now up to the implementation of that interface if the queues are created asynchronously in a sequential order or even in parallel.

snippet: 5to6-QueueCreation

See also [transport-specific queue creation](/transports/msmq/operations-scripting.md#create-queues).


## [Features](/nservicebus/pipeline/features.md)


### Removed FeaturesReport

`FeaturesReport` exposed reporting information about features of a running endpoint instance. It has been internalized. Similarly to previous versions the information is still available by inspecting the `DisplayDiagnosticsForFeatures` logger when the endpoint runs with log level [`DEBUG`](/nservicebus/logging/#logging-levels).


### [Feature Dependencies](/nservicebus/pipeline/features.md#dependencies)

Feature Dependencies, using the string API, now need to be declared using the target feature's full type name (`Type.FullName`) which includes the namespace. Removing the `Feature` suffix is no longer required.

snippet: 5to6-DependentFeature


## Satellites


### ISatellite and IAdvancedSatellite interfaces are obsolete

Both the `ISatellite` and the `IAdvancedSatellite` interfaces are deprecated. The same functionality is available via the `AddSatelliteReceiver` method on the context passed to the [features](/nservicebus/pipeline/features.md#feature-api) `Setup` method. The [satellite documentation](/nservicebus/satellites/) shows this in detail.


### Performance Counters

Satellite pipelines (e.g. retries or timeouts) no longer create Performance Counters. Endpoints provide a single performance counter instance related to the main message processing pipeline.

To learn more about using Performance Counters, refer to the [Performance Counter Usage](/samples/performance-counters/) sample and [Performance Counters](/monitoring/metrics/performance-counters.md) article.


## Transport Seam

`IDequeueMessages` has been obsoleted and is replaced by `IPushMessages`. The interfaces are equivalent so if implementing a transport, implement the new interface. `PushContext` has been given a new property `PushContext.ReceiveCancellationTokenSource`, revealing the intent of cancellation for receiving the current message. The transport implementation should act accordingly, canceling the receive when the source's token is canceled.

The `ConfigureTransport` class was deprecated. Custom transports are now configured using the `TransportDefinition` class, see [Custom Transport Sample](/samples/custom-transport) for more information.


## Corrupted messages

The core will now pass the error queue address to the transport to make it easier to handle corrupted messages. If a corrupted message is detected the transport is expected to move the message to the specified error queue.


## Unit of Work

include: uow-access-to-context
