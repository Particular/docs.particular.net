### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default, so any delayed messages already stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the delay infrastructure even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

snippet: rabbitmq-delay-disable-timeout-manager

At this point, the `.Timeouts` and `.TimeoutsDispatcher` exchanges and queues for the endpoint can be deleted from the broker. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Newly created endpoints that have never been deployed without native delayed delivery should also set this value to prevent the timeout manager from running.