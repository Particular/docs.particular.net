When used together with the outbox in a send-only endpoint, the transactional session must be configured with a remote endpoint(processor endpoint) that will manage the outbox on behalf of the send-only endpoint. The remote endpoint can be specified using the `TransactionalSessionOptions`.

snippet: txsession-nsb-txsessionoptions