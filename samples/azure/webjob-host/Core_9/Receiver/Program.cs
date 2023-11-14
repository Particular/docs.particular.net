using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureHost()
    .Build();

var cancellationToken = new WebJobsShutdownWatcher().Token;
using (host)
{
    await host.RunAsync(cancellationToken)
        .ConfigureAwait(false);
}
