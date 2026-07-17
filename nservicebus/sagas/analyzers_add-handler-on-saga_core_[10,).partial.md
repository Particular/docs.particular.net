## AddHandler cannot be used on sagas

* **Rule ID**: NSB0021
* **Severity**: Error
* **Example message**: `AddHandler<MySaga>()` attempts to register a saga type as a regular message handler. Use `AddSaga<MySaga>()` instead.

A saga is also a message handler, so `AddHandler<MySaga>()` compiles. However, it registers only the message handling and omits the saga metadata that NServiceBus uses to correlate messages to saga data.

`AddSaga<MySaga>()` registers that metadata and then performs the same handler registration, so it is the only correct way to register a saga explicitly.
