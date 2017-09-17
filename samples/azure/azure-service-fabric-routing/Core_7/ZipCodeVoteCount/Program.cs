using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;

static class Program
{
    static async Task Main()
    {
        try
        {
            await ServiceRuntime.RegisterServiceAsync("ZipCodeVoteCountType",
                    context => new ZipCodeVoteCountService(context))
                .ConfigureAwait(false);

            ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ZipCodeVoteCountService).Name);

            Thread.Sleep(Timeout.Infinite);
        }
        catch (Exception e)
        {
            ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            throw;
        }
    }
}