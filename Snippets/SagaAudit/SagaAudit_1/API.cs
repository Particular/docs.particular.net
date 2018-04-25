using System.Collections.Generic;
using NServiceBus;

class API
{
    public void Enable()
    {
        #region SagaAuditNew_Enable

        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.AuditSagaStateChanges(
            serviceControlQueue: "particular.servicecontrol@machine");

        #endregion
    }

    public void CustomSerialization()
    {
        #region SagaAuditNew_CustomSerialization

        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.AuditSagaStateChanges(
            serviceControlQueue: "particular.servicecontrol@machine",
            customSagaEntitySerialization: saga =>
            {
                var data = (MySagaData) saga;
                var dictionary = new Dictionary<string, string>
                {
                    ["prop1"] = data.Name,
                    ["prop2"] = data.Address
                };
                return dictionary;
            });

        #endregion
    }

    class MySagaData
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
