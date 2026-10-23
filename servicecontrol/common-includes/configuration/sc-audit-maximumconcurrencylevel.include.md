### ServiceControl.Audit/MaximumConcurrencyLevel

The maximum number of messages that can be concurrently pulled from the message transport.

It is important that the maximum concurrency level be incremented only if there are no verified bottlenecks in CPU, RAM, network I/O, storage I/O, and storage index lag. Higher numbers can result in faster audit message ingestion, but also consume more server resources, and can increase costs in the case of cloud transports that have associated per-operation costs. In some cases, the ingestion rate can be too high and the underlying database cannot keep up with indexing the new messages. In this case, consider lowering the maximum concurrency level to a value that still allows a suitable ingestion rate while easing the pressure on the database.

Cloud transports with higher latency can benefit from higher concurrency values, but costs can increase as well. Local transports using fast local SSD drives and low latency do not benefit as much.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXIMUMCONCURRENCYLEVEL` |
| **App config key** | `ServiceControl.Audit/MaximumConcurrencyLevel` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `32` in 4.12.0<br/>`10` in earlier versions |
