using System;
using System.Threading.Tasks;
using NServiceBus;
using Quartz;

#region SendMessageJob

public class SendMessageJob :
    IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var endpointInstance = context.EndpointInstance();
            var message = new MyMessage();
            await endpointInstance.Send("Samples.QuartzScheduler.Receiver", message);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Execution Failed: {exception.Message}");
            // TODO: handle exception and dont throw.
            // consider implementing a circuit breaker
            throw;
        }
    }
}

#endregion