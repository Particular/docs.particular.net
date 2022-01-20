using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class TestService : IHostedService
{
    ILogger<TestService> logger;

    public TestService(ILogger<TestService> l)
    {
        logger = l;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task StartAsync(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        AppDomain.CurrentDomain.FirstChanceException += (s, ea) => logger.LogWarning(ea.Exception, $"FirstChanceException: {ea.Exception.Message}");

        //Problem();
        //Workaround();
        //Workaround2();
        //Link();

        //Simple2();
    }

    private void Simple1()
    {
        var ActivitySource = new ActivitySource("Test");
        string traceId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            traceId = produce.Id;
        }

        using (var outer = ActivitySource.StartActivity("Consume", ActivityKind.Consumer, traceId))
        {
        }
    }

    private void Simple2()
    {
        var ActivitySource = new ActivitySource("Test");
        string traceId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            traceId = produce.Id;
            logger.LogError("Producing...");
        }

        using (var outer = ActivitySource.CreateActivity("Consume", ActivityKind.Consumer))
        {
            outer.SetParentId(traceId);
            outer.Start();
            logger.LogError("Consuming...");
        }
    }

    private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
        Console.WriteLine(e.Exception);
    }

    private void Problem()
    {
        var ActivitySource = new ActivitySource("Test");
        string traceId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            traceId = produce.Id;
        }

        // For diagnostics tracing the whole task is desirable to include the fetch operation
        using (var outer = ActivitySource.StartActivity("Consume", ActivityKind.Consumer))
        {

            // Should be part of the trace as the operation could take a while due to unknown technical reasons
            using (var fetch = ActivitySource.StartActivity("Fetch"))
            {
                // Read data and obtain traceID
            }

            // Simulate retrieval of traceId
            outer.SetParentId(traceId).Start(); // <-- No effect

            using (var inner = ActivitySource.StartActivity("Inner"))
            {
            }

            using (var commit = ActivitySource.StartActivity("Commit"))
            {
            }
        }
    }

    public void Workaround()
    {
        var ActivitySource = new ActivitySource("Test");
        string parentId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            parentId = produce?.Id;
        }

        // For diagnostics tracing the whole task is desirable to include the fetch operation
        using (var outer = ActivitySource.CreateActivity("Consume", ActivityKind.Consumer))
        {
            outer?.SetStartTime(DateTime.UtcNow); // Defer start until we know the parentId, but do capture start time
                                                  // Start transaction

            // Should be part of the trace as the operation could take a while due to unknown technical reasons
            using (var fetch = ActivitySource.CreateActivity("Fetch", ActivityKind.Internal))
            {
                fetch?.SetStartTime(DateTime.UtcNow);

                // Read data and obtain traceID

                outer?.SetParentId(parentId);
                outer?.Start();

                fetch?.SetParentId(outer.Id);
                fetch?.Start();
            }

            // Simulate retrieval of traceId
            using (var inner = ActivitySource.StartActivity("Inner"))
            {
            }

            using (var commit = ActivitySource.StartActivity("Commit"))
            {
            }
        }
    }


    public void Workaround2()
    {
        var ActivitySource = new ActivitySource("Test");
        string traceId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            traceId = produce?.Id;
        }

        // For diagnostics tracing the whole task is desirable to include the fetch operation
        using (var outer = ActivitySource.StartActivity("Consume", ActivityKind.Consumer, traceId))
        {
            // Start transaction

            // Should be part of the trace as the operation could take a while due to unknown technical reasons
            using (var fetch = ActivitySource.StartActivity("Fetch", ActivityKind.Internal))
            {
                // Read data and obtain traceID
            }

            // Simulate retrieval of traceId
            using (var inner = ActivitySource.StartActivity("Inner"))
            {
            }

            using (var commit = ActivitySource.StartActivity("Commit"))
            {
            }
        }
    }

    void Link()
    {
        var ActivitySource = new ActivitySource("Test");
        string traceId;

        using (var produce = ActivitySource.StartActivity("Produce", ActivityKind.Producer))
        {
            traceId = produce.Id;
        }

        using (var outer = ActivitySource.StartActivity("Consume"))
        {
            // Should be part of the trace as the operation could take a while due to unknown technical reasons
            using (var fetch = ActivitySource.StartActivity("Fetch"))
            {
                // Read data and obtain incomingTraceId 
            }

            using (var inner = ActivitySource.StartActivity("Process", ActivityKind.Consumer, traceId,
                links: new ActivityLink[] { new ActivityLink(Activity.Current.Context) }))
            {
                // Consume
                using (var commit = ActivitySource.StartActivity("SendSomething"))
                {
                }
            }

            using (var commit = ActivitySource.StartActivity("Commit"))
            {
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}



