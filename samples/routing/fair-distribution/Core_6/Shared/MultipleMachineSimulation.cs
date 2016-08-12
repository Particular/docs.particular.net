using System.Text;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Routing;
using NServiceBus.Transport;

public static class MultipleMachineSimulation
{
    public static void SimulateMultipleMachines<T>(this TransportExtensions<T> transport, string simulatedLocalMachine)
        where T : TransportDefinition
    {
        var settings = transport.GetSettings();
        var endpointName = settings.Get<string>("NServiceBus.Routing.EndpointName");
        var endpointInstance = new EndpointInstance(endpointName);
        settings.Set<EndpointInstance>(endpointInstance.SetProperty("_machine", simulatedLocalMachine));

        #region AddressTranslationRule
        transport.AddAddressTranslationRule(x =>
        {
            string machine;
            var hasMachine = x.EndpointInstance.Properties.TryGetValue("_machine", out machine);
            if (!hasMachine)
            {
                machine = simulatedLocalMachine;
            }

            var queue = new StringBuilder(x.EndpointInstance.Endpoint.ToString());
            if (x.EndpointInstance.Discriminator != null)
            {
                queue.Append($"-{x.EndpointInstance.Discriminator}");
            }
            if (x.Qualifier != null)
            {
                queue.Append($".{x.Qualifier}");
            }
            //Use -at- instead of @ to place all queues on same machine
            return $"{queue}-at-{machine}";
        });
        #endregion
    }
}
