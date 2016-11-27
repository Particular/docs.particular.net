### Legacy .Retries message receiver

In Versions 5 and below, the [Delayed Retries](/nservicebus/recoverability/#delayed-retries) used the `[endpoint_name].Retries` queue for persistent storage of messages to be retried. To prevent message loss when upgrading to Version 6, a dedicated `.Retries` queue receiver is started if not explicitly disabled. It serves a purpose of forwarding messages from the `.Retries` queue to the endpoint's main queue to be retried appropriately.

NOTE: The receiver is only needed during the upgrade from Versions 5 and below and is not needed for new endpoints using Version 6. For details on upgrade process and how to safely disable the receiver refer to: [Version 6 Upgrade Guide](/nservicebus/upgrades/5to6/recoverability.md#legacy-retries-queue).

Letting the receiver continue to run might have negative performance implications depending on the transport being used. For example, for endpoints using either [SQL Server](/nservicebus/sqlserver/) or [Msmq](/nservicebus/msmq/) as its transport, endpoint will periodically poll the `.Retries` queue to check for messages.

The `.Retries` receiver can be disabled via code using:

snippet: 5to6-DisableLegacyRetriesSatellite

INFO: This API is obsoleted in Version 6 and to use it you either have to use [pragma warning](https://msdn.microsoft.com/en-us/library/441722ys.aspx) to disable it or upgrade to Version 6.1.
