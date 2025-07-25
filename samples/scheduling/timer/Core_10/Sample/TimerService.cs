using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using static Program;

public class TimerService(IMessageSession messageSession, ILogger<TimerService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region ScheduleUsingTimer
        var interval = TimeSpan.FromSeconds(5);

        var timer = new Timer(async state =>
        {
            try
            {
                await messageSession.SendLocal(new MyScheduledTask());

                logger.LogInformation("{Task} scheduled", nameof(MyScheduledTask));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, nameof(MyScheduledTask) + " could not be scheduled");
            }
        }, null, interval, interval);

        #endregion

        // timer.Dispose();
        return Task.CompletedTask;
    }
}
