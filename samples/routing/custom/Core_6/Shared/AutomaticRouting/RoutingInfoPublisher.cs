using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Settings;

class RoutingInfoPublisher :
    FeatureStartupTask
{
    RoutingInfoCommunicator dataBackplane;
    IReadOnlyCollection<Type> hanledCommandTypes;
    IReadOnlyCollection<Type> handledEventTypes;
    ReadOnlySettings settings;
    TimeSpan heartbeatPeriod;
    RoutingInfo publication;
    Timer timer;

    public RoutingInfoPublisher(RoutingInfoCommunicator dataBackplane, IReadOnlyCollection<Type> hanledCommandTypes, IReadOnlyCollection<Type> handledEventTypes, ReadOnlySettings settings, TimeSpan heartbeatPeriod)
    {
        this.dataBackplane = dataBackplane;
        this.hanledCommandTypes = hanledCommandTypes;
        this.handledEventTypes = handledEventTypes;
        this.settings = settings;
        this.heartbeatPeriod = heartbeatPeriod;
    }

    protected override Task OnStart(IMessageSession context)
    {
        var mainLogicalAddress = settings.LogicalAddress();
        publication = new RoutingInfo
        {
            EndpointName = settings.EndpointName(),
            TransportAddress = settings.LocalAddress(),
            HandledCommandTypes = hanledCommandTypes.Select(m => m.AssemblyQualifiedName).ToArray(),
            HandledEventTypes = handledEventTypes.Select(m => m.AssemblyQualifiedName).ToArray(),
            Active = true,
        };

        timer = new Timer(state =>
        {
            Publish()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }, null, TimeSpan.Zero, heartbeatPeriod);

        return Publish();
    }

    Task Publish()
    {
        publication.Timestamp = DateTime.UtcNow;
        var dataJson = JsonConvert.SerializeObject(publication);
        return dataBackplane.Publish(dataJson);
    }

    protected override Task OnStop(IMessageSession context)
    {
        using (var waitHandle = new ManualResetEvent(false))
        {
            timer.Dispose(waitHandle);
            waitHandle.WaitOne();
        }

        publication.Active = false;
        return Publish();
    }
}