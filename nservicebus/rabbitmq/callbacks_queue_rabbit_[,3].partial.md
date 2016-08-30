Callbacks are enabled by default, and the transport will create a separate callback receiver queue, named `{endpointname}.{machinename}`, to which all callbacks are routed.


## DisableCallbackReceiver

If callbacks are not being used, the callback receiver can be disabled using the following setting:

snippet:rabbitmq-config-disable-callback-receiver

This means that the queue will not be created and no extra threads will be used to fetch messages from that queue.


## CallbackReceiverMaxConcurrency

By default, 1 dedicated thread is used for the callback receiver queue. To add more threads, due to a high rate of callbacks, use the following:

snippet:rabbitmq-config-callbackreceiver-thread-count