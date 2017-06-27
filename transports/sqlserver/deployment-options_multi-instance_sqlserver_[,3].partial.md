### Multi-instance

WARNING: The multi-instance option is deprecated and not recommended for new projects.

WARNING: Although Versions 3 and above offer support for multi-instance mode when using DTC, Outbox is not a supported option when running in multi-instance mode.

 * Requires storing business data in the same database as Outbox data.
 * Requires DTC or Outbox (if available).


### Multi-instance with store-and-forward

WARNING: This option is deprecated and not recommended for new projects.

SQL Server transport does not support store-and-forward mechanism natively. Therefore, if the receiving endpoint's infrastructure e.g. DTC or SQL Server instance is unavailable especially in a *multi-instance* mode, messages to the endpoint can't be delivered. The sending endpoint and all the other endpoints that depend on it will also be unavailable. The problem can be addressed by using the [Outbox](/nservicebus/outbox/) feature.

The Outbox feature can be used to avoid escalating transactions to DTC, when each endpoint has a separate database for storing queues and business data on the same SQL Server instance. However, it's not possible to avoid distributed transactions when any of the queues are on a different SQL Server instance or catalog. That means that in order to avoid escalation, each endpoint should have their dedicated error and audit queues.

When using Outbox:

 * Messages are not dispatched immediately after the `Send()` method is called. Instead, they are first stored in the Outbox table in the same database that the endpoint's persistence is using. After the handler logic completes successfully, the messages stored in the Outbox table are forwarded to their final destinations.
 * If any of the forward operations fails, the message sending will be retried using the standard [recoverability mechanism](/nservicebus/recoverability/). Attempting to retry the forward operation may result in dispatching some messages multiple times. However, the Outbox feature automatically de-duplicates the incoming messages based on their IDs, therefore providing `exactly-once` message delivery. The receiving endpoint also has to be configured to use the Outbox.
