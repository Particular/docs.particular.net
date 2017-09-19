using NServiceBus.Persistence;
using NServiceBus.Settings;
using NServiceBus.Transport;

public class AzureStorageQueueTransport :
    TransportDefinition
{
    public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        throw new System.NotImplementedException();
    }

    public override string ExampleConnectionStringForErrorMessage
    {
        get { throw new System.NotImplementedException(); }
    }
}

public class AzureServiceBusTransport :
    TransportDefinition
{
    public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        throw new System.NotImplementedException();
    }

    public override string ExampleConnectionStringForErrorMessage
    {
        get { throw new System.NotImplementedException(); }
    }
}

public class AzureStoragePersistence :
    PersistenceDefinition
{
}