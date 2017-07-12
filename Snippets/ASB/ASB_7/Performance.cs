using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

class Performance
{
    async Task SendPerformance()
    {
        var endpointConfiguration = new EndpointConfiguration("whatever");
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region asb-slow-send

        for (var i = 0; i < 100; i++)
        {
            // by awaiting each individual send, no client side batching can take place
            // latency is incurred for each send and thus negatively impacts performance
            await endpointInstance.Send(new SomeMessage())
                .ConfigureAwait(false);
        }

        #endregion

        #region asb-fast-send
        var tasks = new List<Task>(10000);
        for (var i = 0; i < 10000; i++)
        {
            var task = endpointInstance.Send(new SomeMessage());
            tasks.Add(task);
        }

        // by awaiting the sends as one unit, this code allows the ASB SDK's client side batching to kick in and bundle sends
        // this results in less latency overhead per individual sends and thus improved performance
        await Task.WhenAll(tasks)
            .ConfigureAwait(false);
        #endregion
    }
}

public class SomeMessage :
    IMessage
{
}