namespace Billing
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    class NetworkLatencySimulationBehavior : Behavior<IDispatchContext>
    {
        SimulationEffects simulationEffects;

        public NetworkLatencySimulationBehavior(SimulationEffects simulationEffects)
        {
            this.simulationEffects = simulationEffects;
        }

        public override async Task Invoke(IDispatchContext context, Func<Task> next)
        {
            await simulationEffects.SimulateNetworkLatency()
                .ConfigureAwait(false);

            await next()
                .ConfigureAwait(false);
        }
    }
}