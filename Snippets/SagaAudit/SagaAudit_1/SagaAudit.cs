using System.Collections.Generic;
using NServiceBus;

class MyClass
{
    public void Enable()
    {
        #region SagaAudit_Enable

        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.AuditSagaStateChanges(
            serviceControlQueue: "ServiceControl_Queue");

        #endregion
    }

    public void CustomSerialization()
    {
        #region SagaAudit_CustomSerialization

        var endpointConfiguration = new BusConfiguration();
        endpointConfiguration.AuditSagaStateChanges(
            serviceControlQueue: "ServiceControl_Queue",
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