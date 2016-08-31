### Legacy .Retries message receiver

In Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) of NServiceBus used the `[endpoint_name].Retries` queue for persistent storage of messages to be retried. To prevent message loss when upgrading in Version 6, a dedicated .retries queue receiver is started if not explicitly disabled. It serves a purpose of forwarding messages from the `.retries` queue to the endpoint's main queue to be retried appropriately.

NOTE: The receiver is needed only during the upgrade from Versions 5 and below and is not needed for new endpoints using Version 6. For details on upgrade process and how to safely disable the receiver refer to: [Version 5 Upgrade Guide](/nservicebus/upgrades/5to6-recoverability.md#legacy-retries-queue).

Letting the receiver run might have negative performance implications depending on the transport. For endpoints using [SQL Server](/nservicebus/sqlserver/) or [Msmq](/nservicebus/msmq/) Transports it will result in periodic polling to check for messages in the .retries queue.

The `.Retries` can be disabled via code using:

snippet: 5to6-DisableLegacyRetriesSatellite

INFO: This configuration API will be obsoleted and removed in Version 7.