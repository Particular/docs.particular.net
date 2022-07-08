## Controlling queue type

The routing topologies provided by the transport can work with classic queues or quorum queues by specifying it via the `QueueType` parameter. When installers are enabled, this parameter controls what type of queue is created.

For existing endpoints using classic queues, the [`queue migrate-to-quorum`](operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool can be used to migrate to quorum queues on a queue-by-queue basis.