namespace NServiceBus.MyPersistence;

using NServiceBus.Persistence;

class MyPersistence : PersistenceDefinition, IPersistenceDefinitionFactory<MyPersistence>
{
    static MyPersistence IPersistenceDefinitionFactory<MyPersistence>.Create() => new();
}