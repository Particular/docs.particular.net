using System;
using System.Threading.Tasks;
using NServiceBus;
using Quartz;
using Serilog;

#region SendMessageJob

public class SendMessageJob :
    IJob
{
    static ILogger log = Log.ForContext<SendMessageJob>();

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var endpointInstance = context.EndpointInstance();
            var message = new MyMessage();
            await endpointInstance.Send("Samples.QuartzScheduler.Receiver", message)
                .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            log.Fatal(exception, "Execution Failed");
            // TODO: handle exception and dont throw.
            // consider implementing a circuit breaker
            throw;
        }
    }
}

#endregion