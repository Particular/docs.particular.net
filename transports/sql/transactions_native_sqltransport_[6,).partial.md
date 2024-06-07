There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

> [!NOTE]
> This transaction is not shared outside of the message receiver. That means there is a possibility of persistent side-effects when processing fails, i.e. [ghost messages](/nservicebus/concepts/glossary.md#ghost-message) might occur.

 * **SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but transaction is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.

> [!WARNING]
> If using SqlServer for [transport](/transports/sql/) and [persistence](/persistence/sql/dialect-mssql) with different connection strings, without [outbox](/nservicebus/outbox/) enabled, in these transaction modes [the persistence uses the connection and transaction context established by the transport when accessing saga data](/persistence/sql/dialect-mssql#connection-sharing).