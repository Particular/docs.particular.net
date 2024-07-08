Executing code at various intervals is a common need when building enterprise systems. The following options are available for scheduling:

- A [PeriodicTimer](https://learn.microsoft.com/en-us/dotnet/api/system.threading.periodictimer). See the [PeriodicTimer sample](/samples/scheduling/periodictimer/).
- A [.NET Timer](https://msdn.microsoft.com/en-us/library/system.threading.timer.aspx).
- [NServiceBus sagas](/nservicebus/sagas/)
- [Quartz.NET](https://www.quartz-scheduler.net/). See the [Quartz.NET sample](/samples/scheduling/quartz/).
- OS task scheduler, like the Windows task scheduler or Linux cron jobs.
- [Timer trigger for Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer)
- [Hangfire](https://www.hangfire.io/). See the [Hangfire sample](/samples/scheduling/hangfire/).
