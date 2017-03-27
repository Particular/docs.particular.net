using System;
using System.Runtime.Serialization;

[DataContract(Namespace = "NServiceBus.Persistence.ServiceFabric", Name = "SagaEntry")]
public sealed class SagaEntry : IExtensibleDataObject
{
    public SagaEntry(string data, Version sagaTypeVersion, Version persistenceVersion)
    {
        Data = data;
        SagaTypeVersion = sagaTypeVersion;
        PersistenceVersion = persistenceVersion;
    }

    [DataMember(Name = "Data", Order = 0)]
    public string Data { get; private set; }

    [DataMember(Name = "PersistenceVersion", Order = 1)]
    public Version PersistenceVersion { get; set; }

    [DataMember(Name = "SagaTypeVersion", Order = 2)]
    public Version SagaTypeVersion { get; set; }

    public ExtensionDataObject ExtensionData { get; set; }
}