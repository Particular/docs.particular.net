SQL Server transport monitors each input queue separately. The monitoring task is responsible for detecting the number of messages waiting for delivery and creating receive tasks - one for each pending message.

The maximum number of concurrent tasks will never exceed the value set by `LimitMessageProcessingConcurrencyTo`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.

Read more information about [tuning endpoint message processing](/nservicebus/operations/tuning.md).
