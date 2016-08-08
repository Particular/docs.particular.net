There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

NOTE: This transaction is not shared outside of the message receiver. That means there is a possibility of persistent side-effects when processing fails, i.e. *ghost messages* might occur.

 * ** SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but transaction is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.
to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.
