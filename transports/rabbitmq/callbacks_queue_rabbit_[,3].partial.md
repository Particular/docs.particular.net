When sending a message, a callback can be registered that will be invoked when a response arrives.

When scaling out an endpoint, any of the endpoint instances can consume messages from the same shared broker queue. This behavior can cause problems when dealing with reply messages for which a callback has been registered. The reply message needs to go to the specific instance that registered the callback.

Callback support is enabled by default, and the transport will create a separate callback receiver queue, named `{endpointname}.{machinename}`. This queue is used as `reply-to` address for all outgoing messages for which response callback has been registered.


## DisableCallbackReceiver

If callbacks are not being used, the callback receiver can be disabled using the following setting:

snippet: rabbitmq-config-disable-callback-receiver

This means that the queue will not be created and no extra threads will be used to fetch messages from that queue.


## CallbackReceiverMaxConcurrency

By default, a single dedicated thread is used for the callback receiver queue. This should be enough for most scenarios. If the endpoint is meant to handle large number of callback-bound messages, use the following snippet to increase the number of threads servicing the callback queue:

snippet: rabbitmq-config-callbackreceiver-thread-count
