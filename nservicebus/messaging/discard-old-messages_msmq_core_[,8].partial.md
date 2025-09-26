### MSMQ transport

MSMQ continuously checks the TTBR of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space gets reclaimed.

> [!NOTE]
> MSMQ enforces a single TTBR value for all messages in a transaction. To prevent message loss, `TimeToBeReceived` is not supported for endpoints with [transaction mode](/transports/transactions.md) `SendsAtomicWithReceive` or `TransactionScope` by default.

> [!WARNING]
> Due to a bug in Version 6, `TransportTransactionMode.ReceiveOnly` wrongly enlisted all outgoing messages in the same transaction causing the issues described above.

For more details about how the MSMQ transport handles TTBR, see [discarding expired messages in MSMQ](/transports/msmq/discard-expired-messages.md).