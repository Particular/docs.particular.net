
The total number of possible retries can be calculated with the following formula

```txt
Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
```

Given a variety of immediate and delayed configuration values here are the resultant possible attempts.

| ImmediateRetries | DelayedRetries | Total possible attempts |
|------------------|----------------|-------------------------|
| 0                | 0              | 1                       |
| 1                | 0              | 2                       |
| 2                | 0              | 3                       |
| 3                | 0              | 4                       |
| 0                | 1              | 2                       |
| 1                | 1              | 4                       |
| 2                | 1              | 6                       |
| 3                | 1              | 8                       |
| 1                | 2              | 6                       |
| 2                | 2              | 9                       |
| 1                | 3              | 8                       |
| **3**            | **5**          | **24  (default)**       |

### Scale-out multiplier

NOTE: Retry behavior can be interpreted as if retries result in duplicates when scaled-out. Retry behavior can result in excessive processing attempts but no duplicate messages are created. Ensure that logging uses unique identifiers for each endpoint instance.

If an endpoint is scaled-out the number of processing attempts increase if instances are retrieving messages from the same queue and the transport does not have a native delivery counter.

Affected transports:

- Azure Storage Queues
- SQL Server
- RabbitMQ
- Amazon SQS
- MSMQ (only if running multiple instance on the same machine)

Unaffected transports:

- Azure Service Bus
- Azure Service Bus Legacy

Azure Service Bus transports use a native delivery counter which is incremented after any endpoint instance fetches a message from a (shared) queue. The native delivery counter guarantees that the retry number is the same regardless if the endpoint is scaled out.


The number of instances acts as a multiplier for the maximum number of attempts.

```txt
Minimum Attempts = (ImmediateRetries:NumberOfRetries + 1) * (DelayedRetries:NumberOfRetries + 1)
Maximum Attempts = MinimumAttempts * NumberOfInstances
```

Example:

When taking the default values for immediate and delayed retries (five and three, respectively) and 6 instances the total number of attempts will be a minimum of `(5+1)*(3+1)=24` attempts and a maximum of `24*6=144` attempts.

