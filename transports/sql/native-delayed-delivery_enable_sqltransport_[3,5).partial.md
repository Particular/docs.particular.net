To enable native delayed delivery, use the following API:

snippet: EnableNativeDelayedDelivery

NOTE: In this mode, the timeout manager will still be running in order to process all outstanding delayed messages. Refer to the [Disabling the timeout manager](/transports/sql/native-delayed-delivery.md#backwards-compatibility) section for details on how to disable the timeout manager entirely.