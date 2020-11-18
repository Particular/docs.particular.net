SQL Server transport operates in two modes: *peek* and *receive*. It starts in the *peek* mode and checks, using a `select count` query the number of pending messages. If the number is greater then zero, it switches to the *receive* mode and starts spawning receive tasks that use the `delete` command to receive messages.

The maximum number of concurrent receive tasks never exceeds the value set by `LimitMessageProcessingConcurrencyTo` (the number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms).

When all tasks are done the transport switches back to the *peek* mode. 

In certain conditions the initial estimate of number of pending messages might be wrong e.g. when there is more than one instance of a scaled-out endpoint consuming messages from the same queue. In this case one of the receive tasks is going to fail (`delete` returns no results). When this happens, the transport immediately switches back to the *peek* mode.

The default peek interval, if there is has been no messages in the queue, is 1 second. The recommended range for this setting is between 100 milliseconds to 10 seconds. If a value higher than the maximum recommended settings is used, a warning message will be logged. While a value less than 100 milliseconds will put too much unnecessary stress on the database, a value larger than 10 seconds should also be used with caution as it may result in messages backing up in the queue. 

### Queue peek settings

#### Peek delay configuration

Use the following code:

snippet: sqlserver-queue-peeker-config-delay

#### Peek batch size configuration

Use the following code:

snippet: sqlserver-queue-peeker-config-batch-size

Read more information about [tuning endpoint message processing](/nservicebus/operations/tuning.md).
