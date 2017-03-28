using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;

static class Program
{
    static void Main()
    {
        try
        {
            ServiceRuntime.RegisterServiceAsync("ZipCodeVoteCountType",
                context => new ZipCodeVoteCountService(context)).GetAwaiter().GetResult();

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
