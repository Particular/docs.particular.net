using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

public class WorkProcessingHandler : IHandleMessages<ProcessWorkOrder>
{
    public async Task Handle(ProcessWorkOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Processing work order '{message.WorkOrder}'");

        try
        {
            PerformJob();

            await context.Reply(new WorkOrderCompleted
            {
                ProcessId = message.ProcessId,
                WorkOrderNo = message.WorkOrder,
                Status = WorkStatus.Success
            });
        }
        catch (Exception ex)
        {
            await context.Reply(new WorkOrderCompleted
            {
                ProcessId = message.ProcessId,
                WorkOrderNo = message.WorkOrder,
                Status = WorkStatus.Failed,
                Error = ex.Message
            });
        }
    }

    private void PerformJob()
    {
        // emulate 5% failure
        var shouldFail = Random.Shared.Next(20);
        if (shouldFail == 1)
        {
            throw new Exception("Something went wrong during processing.");
        }

        Thread.Sleep(Random.Shared.Next(500, 1000));
    }
}