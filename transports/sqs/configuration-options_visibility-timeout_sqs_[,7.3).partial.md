## Message visibility

The message visibility timeout in Amazon SQS defines how long a message remains hidden from other consumers after being received. If a handler takes longer to process a message than the configured visibility timeout, the message becomes visible again and may be reprocessed.

Without a message visibility extension mechanism (supported in version 7.3 and above), this can result in:

- Duplicate message processing, wasting resources
- Message deletion failures, causing the message to reappear on the queue
- Inaccurate delivery attempt counters, impacting poison message detection

To avoid these issues, configure the visibility timeout at the queue level, when creating, to at least match the maximum expected processing time of a message.
