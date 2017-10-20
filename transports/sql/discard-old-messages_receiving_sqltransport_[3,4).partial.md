The messages with elapsed Time-To-Be-Received (TTBR) are ignored by the message pump whose receive query has a `where` clause that excludes expired messages.

The expired messages are removed from the queue by a scheduled job which runs every 5 minutes. Each run the job attempts to remove all expired messages, in batches, using series a `delete` statements. Each batch is limited to 10000 messages.