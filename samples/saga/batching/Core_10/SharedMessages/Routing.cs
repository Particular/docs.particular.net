using System.Threading.Tasks;
using NServiceBus;

public static class Endpoint
{
    public static Task<IEndpointInstance> StartWithDefaultRoutes(this EndpointConfiguration config, RoutingSettings<LearningTransport> routing)
    {
        routing.RouteToEndpoint(typeof(StartProcessing), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(WorkOrderCompleted), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(WorkAllDone), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(ProcessWorkOrder), EndpointNames.WorkProcessor);

        return NServiceBus.Endpoint.Start(config);
    }
}