## Enable the timeout manager

To assist with the upgrade process when upgrading from an older version of the transport that doesn't support native-delayed delivery, the timeout manager can be enabled. Any delayed messages stored in the endpoint's persistence database before the upgrade are sent when their timeouts expire. Any delayed messages sent after the upgrade are sent through the native delayed delivery infrastructure, even though the timeout manager is enabled. The timeout manager migration mode can be enabled with:

snippet: DelayedDeliveryEnableTM

Once an endpoint has no more delayed messages in its persistence database, there is no need for the timeout manager. It can be disabled by removing the above enable call.

At this point, all `.Timeouts` and `.TimeoutsDispatcher` tables for the endpoint can be deleted from the database. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Timeout Manager should also be disabled for newly created endpoints, i.e. endpoints that use the native delayed delivery implementation and haven't been deployed yet.