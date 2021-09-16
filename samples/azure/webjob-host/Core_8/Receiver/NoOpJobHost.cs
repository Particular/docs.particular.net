using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

public class NoOpJobHost : IJobHost
{
    public Task CallAsync(string name, IDictionary<string, object> arguments = null, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }
}