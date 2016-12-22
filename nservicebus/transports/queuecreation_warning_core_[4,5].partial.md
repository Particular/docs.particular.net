
WARNING: NServiceBus will automatically request the transport to create queues needed if the [installers](/nservicebus/operations/installers.md) are enabled. This also includes queues needed by all declared [satellites](/nservicebus/satellites). Prefer the use of scripts to create custom queues instead of relying on the `IWantQueuesCreated` interface provided by NServiceBus.
