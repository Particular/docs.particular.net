using System.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;

public class WorkerRole :
    RoleEntryPoint
{
    NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

    public override bool OnStart()
    {
        nsb.Start();

        Trace.TraceInformation("HostWorker has been started");

        return base.OnStart();
    }

    public override void OnStop()
    {
        Trace.TraceInformation("HostWorker is stopping");

        nsb.Stop();
        base.OnStop();

        Trace.TraceInformation("HostWorker has stopped");
    }
}