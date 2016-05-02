using NServiceBus.Persistence;
using NServiceBus.Settings;
using NServiceBus.Transports;

public class AzureStorageQueueTransport : TransportDefinition
{
    protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        throw new System.NotImplementedException();
    }

    public override string ExampleConnectionStringForErrorMessage
    {
        get { throw new System.NotImplementedException(); }
    }
}
public class AzureServiceBusTransport : TransportDefinition
{
    protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        throw new System.NotImplementedException();
    }

    public override string ExampleConnectionStringForErrorMessage
    {
        get { throw new System.NotImplementedException(); }
    }
}
public class AzureStoragePersistence : PersistenceDefinition
{
}
