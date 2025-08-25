Scheduling code execution at specific intervals is a common requirement in enterprise systems, such as for recurring tasks, maintenance jobs, or periodic data processing. Several approaches are available for implementing scheduling in .NET and NServiceBus-based solutions:

- **[PeriodicTimer](https://learn.microsoft.com/en-us/dotnet/api/system.threading.periodictimer):** A lightweight .NET API for running code at regular intervals. See the [PeriodicTimer sample](/samples/scheduling/periodictimer/).
- **[System.Threading.Timer](https://msdn.microsoft.com/en-us/library/system.threading.timer.aspx):** A built-in .NET timer for scheduling recurring or delayed tasks.
- **[NServiceBus Sagas](/nservicebus/sagas/):** Use sagas to coordinate long-running, scheduled, or recurring workflows within NServiceBus.
- **[Quartz.NET](https://www.quartz-scheduler.net/):** A powerful, open-source job scheduling library for .NET. See the [Quartz.NET sample](/samples/scheduling/quartz/).
- **Operating System Task Schedulers:** Use tools like Windows Task Scheduler or Linux cron jobs to run scripts or applications at scheduled times.
- **[Azure Functions Timer Trigger](https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer):** Schedule serverless functions to run on a timer in Azure.
- **[Hangfire](https://www.hangfire.io/):** A background job scheduler for .NET applications. See the [Hangfire sample](/samples/scheduling/hangfire/).

Choose the approach that best fits your application's requirements and hosting environment.
