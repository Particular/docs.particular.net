## How it works


### Heartbeats

The heartbeat messages arrive at adapter's `Particular.ServiceControl` queue. From there they are moved to `Particular.ServiceControl.SQL` queue in ServiceControl database. In case of problems (e.g. destination database being down) the forward attempts are repeated configurable number of times after which messages are dropped to prevent the queue from growing indefinitely.

### Audits

The audit messages arrive at adapter's `audit` queue. From there they are moved to `audit` queue in ServiceControl database and are ingested by ServiceControl.


### Retries

If a message fails all recoverability attempts in a business endpoint, it is moved to the `error` queue located in the adapter database. The adapter enriches the message by adding `ServiceControl.RetryTo` header pointing to the adapter's input queue in ServiceControl database (`ServiceControl.SqlServer.Adapter.Retry`). Then the message is moved to the `error` queue in ServiceControl database and ingested into ServiceControl RavenDB store. 

When retrying, ServiceControl looks for `ServiceControl.RetryTo` header and, if it finds it, it sends the message to the queue from that header instead of the ultimate destination.

The adapter picks up the message and forwards it to the destination using its endpoint-facing transport.
