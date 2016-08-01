using System;
using System.Text;
using NServiceBus;
using NServiceBus.Transport;

public static class MultipleMachineSimulation
{
    public static void SimulateMultipleMachines<T>(this TransportExtensions<T> transport, string simulatedLocalMachine)
        where T : TransportDefinition
    {
        #region AddressTranslationRule
        transport.AddAddressTranslationRule(x =>
        {
            string machine;
            var hasMachine = x.EndpointInstance.Properties.TryGetValue("Machine", out machine);
            if (!hasMachine || machine == Environment.MachineName)
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
