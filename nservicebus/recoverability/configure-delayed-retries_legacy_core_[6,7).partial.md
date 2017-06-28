### Legacy .Retries message receiver

In Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) used the `[endpoint_name].Retries` queue for persistent storage of messages to be retried. To prevent message loss when upgrading to Version 6, a dedicated `.Retries` queue receiver is started if not explicitly disabled. It serves a purpose of forwarding messages from the `.Retries` queue to the endpoint's main queue to be retried appropriately.

NOTE: The receiver is only needed during the upgrade from Versions 5 and below and is not needed for new endpoints using Version 6. For details on upgrade process and how to safely disable the receiver refer to the [Version 6 Upgrade Guide](/nservicebus/upgrades/5to6/recoverability.md#legacy-retries-queue). 
Letting the receiver continue to run might have negative performance implications depending on the transport being used. For example, for endpoints using either [SQL Server](/transports/sql/) or [Msmq](/transports/msmq/) as its transport, endpoint will periodically poll the `.Retries` queue to check for messages.

The `.Retries` receiver can be disabled via code using:

snippet: DisableLegacyRetriesSatellite

INFO: This .Retries message receiver and the API to disable it will be removed in Version 7.
