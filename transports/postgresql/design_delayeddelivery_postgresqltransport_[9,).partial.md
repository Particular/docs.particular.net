### Delayed delivery

Delayed messages are stored in a dedicated table. When they are due, the transport moves them to the destination queues in batches. In version 9.2 and later, the batch size is 200 by default, or 100 when [independent movers](#behavior-delayed-delivery-independent-movers) are enabled. In versions 9.0 and 9.1, the default is 100. The batch size can be changed using:

snippet: DelayedDeliveryBatchSizeCustom

For messages with large bodies, lower the batch size to keep each move short.

In version 9.2 and later, only one instance of a scaled-out endpoint moves due delayed messages at a time. Before moving a batch, an instance tries to take a transaction-level advisory lock on the delayed messages table using `pg_try_advisory_xact_lock`. An instance that doesn't get the lock skips the batch and tries again after a random delay between 0.5 and 1 second. This delay isn't configurable. Because instances don't compete for the same rows, they don't deadlock while moving due messages.

The `DelayedDelivery` section of the [startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) shows `SingleMover` set to `true` when the lock is used.

#### Independent movers

To let every endpoint instance move due delayed messages independently, without the lock, enable the `NServiceBus.Transport.PostgreSql.DelayedDelivery.UseIndependentMovers` switch before the endpoint starts:

snippet: UseIndependentMoversSwitch

or in the project file:

snippet: UseIndependentMoversMsBuild

When the switch is enabled, the endpoint logs a warning at startup. The switch will be removed in the next major version.
