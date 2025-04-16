### Asynchronous programming model (APM) pattern

For existing code which uses the [Asynchronous Programming Model (APM)](https://msdn.microsoft.com/en-us/library/ms228963.aspx), it is best to use [`Task.Factory.FromAsync`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.taskfactory.fromasync.aspx) to wrap the `BeginOperationName` and `EndOperationName` methods with a task object.

snippet: HandlerWhichIntegratesWithAPM

### Asynchronous RPC calls

The APM approach described above can be used to integrate with remote procedure calls, as shown in this snippet:

snippet: HandlerWhichIntegratesWithRemotingWithAPM

or use [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) directly in a message handler:

snippet: HandlerWhichIntegratesWithRemotingWithTask

> [!NOTE]
> [`Task.Run`](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.run.aspx) can have significantly less overhead than using a delegate with `BeginInvoke`/`EndInvoke`. Both APIs will use the worker thread pool as the underlying scheduling engine by default. Analyze and measure the business scenarios involved.
