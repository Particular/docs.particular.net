---
title: Extension Seam Changes in NServiceBus Version 6
reviewed: 2020-05-07
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## [Timeout](/nservicebus/sagas/timeouts.md) storage

`IPersistTimeouts` has been split into two interfaces, `IPersistTimeouts` and `IQueryTimeouts`, to separate those storage concerns. Both must be implemented to have a fully functional timeout infrastructure.

`IQueryTimeouts` implements the concern of polling for timeouts outside the context of a message pipeline. `IPersistTimeouts` implements the concern of storage and removal for timeouts which is executed inside the context of a pipeline. Depending on the design of the timeout persisters, those concerns can now be implemented independently. Furthermore, `IPersistTimeouts` introduced a new parameter `TimeoutPersistenceOptions `. This parameter allows access to the pipeline context. This enables timeout persisters to manipulate everything that exists in the context during message pipeline execution.


## [Outbox](/nservicebus/outbox/)

`IOutboxStorage` introduced a new parameter `OutboxStorageOptions`. This parameter provides access to the pipeline context. This enables outbox storage methods to manipulate everything that exists in the context during message pipeline execution.


## [Queue creation](/transports/queuecreation.md)

In NServiceBus version 5, the implementation of the interface `ICreateQueues` was called for each queue that needed to be created. In version 6, `ICreateQueues` has been redesigned. The implementation of the interface is called once but with all queues provided on the `QueueBindings` object. It is now up to the implementation of that interface to determine if the queues are created asynchronously in a sequential order or even in parallel.

snippet: 5to6-QueueCreation

See also [Transport-specific queue creation](/transports/msmq/operations-scripting.md#create-queues).


## [Features](/nservicebus/pipeline/features.md)


### Removed FeaturesReport

`FeaturesReport` exposed reporting information about features of a running endpoint instance. It has been internalized. As with previous versions, the information is still available by inspecting the `DisplayDiagnosticsForFeatures` logger when the endpoint runs with log level [`DEBUG`](/nservicebus/logging/#default-logging-changing-the-defaults-changing-the-logging-level).


### [Feature dependencies](/nservicebus/pipeline/features.md#dependencies)

Feature dependencies, using the string API, are now declared using the target feature's full type name (`Type.FullName`) which includes the namespace. Removing the `Feature` suffix is no longer required.

snippet: 5to6-DependentFeature


## Satellites

### ISatellite and IAdvancedSatellite interfaces are obsolete

Both the `ISatellite` and the `IAdvancedSatellite` interfaces are deprecated. The same functionality is available via the `AddSatelliteReceiver` method on the context passed to the [features](/nservicebus/pipeline/features.md#feature-api) `Setup` method. The [satellite documentation](/nservicebus/satellites/) shows more detail.


### Performance counters

Satellite pipelines (e.g. retries or timeouts) no longer create performance counters. Endpoints provide a single performance counter instance related to the main message processing pipeline.

To learn more about using performance counters, refer to the [performance counter usage](/samples/performance-counters/) sample and the [performance counters](/monitoring/metrics/performance-counters.md) article.


## Transport seam

`IDequeueMessages` is now obsolete and has been replaced by `IPushMessages`. The interfaces are equivalent so when creating a transport, implement the new interface. `PushContext` has been given a new property `PushContext.ReceiveCancellationTokenSource`, revealing the intent of cancellation for receiving the current message. The transport implementation should act accordingly, canceling the receive when the source's token is canceled.

The `ConfigureTransport` class was deprecated. Custom transports are now configured using the `TransportDefinition` class, see the [custom transport sample](/samples/custom-transport) for more information.


## Corrupted messages

The core will now pass the error queue address to the transport to make it easier to handle corrupted messages. If a corrupted message is detected the transport is expected to move the message to the specified error queue.


## Unit of work

include: uow-access-to-context
