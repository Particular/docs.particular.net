SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)s - one for each pending message.

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.
