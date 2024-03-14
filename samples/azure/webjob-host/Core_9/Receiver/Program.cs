using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder()
    .ConfigureHost()
    .Build();

var cancellationToken = new WebJobsShutdownWatcher().Token;
using (host)
{
    await host.RunAsync(cancellationToken);
}
