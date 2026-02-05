
> [!NOTE]
> When using the Outbox, the [transport transaction level must be explicitly set to `ReceiveOnly`](/transports/transactions.md#transaction-modes-transport-transaction-receive-only). This ensures that messages dispatched via the Outbox cannot be rolled back by the transport after the changes have been persisted to the Outbox storage.
