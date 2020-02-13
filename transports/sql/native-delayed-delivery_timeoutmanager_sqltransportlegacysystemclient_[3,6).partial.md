### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default. Any delayed messages stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the native delayed delivery infrastructure, even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

snippet: DelayedDeliveryDisableTM

At this point, all the `.Timeouts` and `.TimeoutsDispatcher` tables for the endpoint can be deleted from the database. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Timeout Manager should be also disabled for newly created endpoints, i.e. the endpoints that use the native delayed delivery implementation and haven't been deployed yet.