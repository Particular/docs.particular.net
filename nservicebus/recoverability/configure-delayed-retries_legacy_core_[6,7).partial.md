### Legacy .Retries message receiver

In versions 5 and below, [delayed retries](/nservicebus/recoverability/#delayed-retries) use the `[endpoint_name].Retries` queue for persistent storage of messages to be retried. To prevent message loss when upgrading to version 6, a dedicated `.Retries` queue receiver is started if not explicitly disabled. It serves the purpose of forwarding messages from the `.Retries` queue to the endpoint's main queue to be retried appropriately.

NOTE: The receiver is needed only during the upgrade from versions 5 and below and is not needed for new endpoints using version 6. For details on the upgrade process and how to safely disable the receiver, refer to the [version 6 upgrade guide](/nservicebus/upgrades/5to6/recoverability.md#legacy-retries-queue). 
Letting the receiver continue to run might have negative performance implications depending on the transport being used. For example, for endpoints using either [SQL Server](/transports/sql/) or [MSMQ](/transports/msmq/) as its transport, an endpoint will periodically poll the `.Retries` queue to check for messages.

The `.Retries` receiver can be disabled via code using:

snippet: DisableLegacyRetriesSatellite

INFO: This .Retries message receiver and the API to disable it have been removed in version 7.
