## Queue name generator

**Optional**

**Default**: `$"{queueNamePrefix}{queueName}` with unsupported characters like `.` are replaced with a hyphen `-`

Provides the ability to override the queue name generation with a custom function that allows creating queues in alignment with custom conventions.

snippet: QueueNameGenerator

Note: The provided function needs to be _idempotent_ i.e. apply the specified prefix only if it has not been applied yet.