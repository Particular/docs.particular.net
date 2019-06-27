using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureHost()
            .Build();

        var cancellationToken = new WebJobsShutdownWatcher().Token;
        using (host)
        {
            await host.RunAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}