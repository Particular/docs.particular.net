
Callbacks are routed to queues based on the machine's hostname. On [broker transports](/transports/types.md#broker-transports) additional queues are created for each endpoint/hostname combination e.g. `Sales.MachineA`, `Sales.MachineB`. Callbacks do not require any additional queue configuration from the user.

WARNING: When more than one instance, of a given endpoint, is deployed to the same machine, the responses might be delivered to the incorrect instance and callback never fires.