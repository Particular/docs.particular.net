## Outbox

The [Outbox](/nservicebus/outbox) feature provides idempotency at the infrastructure level and allows running in *transport transaction* mode while still getting the same semantics as *Transaction scope* mode.

NOTE: Outbox data needs to be stored in the same database as business data to achieve the `exactly-once delivery`.

When using the Outbox, any messages resulting from processing a given received message are not sent immediately but rather stored in the persistence database and pushed out after the handling logic is done. This mechanism ensures that the handling logic can only succeed once so there is no need to design for idempotency.
