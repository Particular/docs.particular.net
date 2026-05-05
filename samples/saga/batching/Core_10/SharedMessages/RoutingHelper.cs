using NServiceBus;

public static class RoutingHelper
{
    public static void ApplyDefaultRouting(RoutingSettings<LearningTransport> routing)
    {
        routing.RouteToEndpoint(typeof(StartProcessing), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(WorkOrderCompleted), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(WorkAllDone), EndpointNames.WorkGenerator);
        routing.RouteToEndpoint(typeof(ProcessWorkOrder), EndpointNames.WorkProcessor);
    }
}