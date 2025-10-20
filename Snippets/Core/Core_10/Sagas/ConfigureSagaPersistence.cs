namespace Core.Sagas;

using NServiceBus;
using NServiceBus.Persistence;

class ConfigureSagaPersistence
{

    void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region saga-configure

        endpointConfiguration.UsePersistence<PersistenceToUseGoesHere>();

        #endregion
    }

    public class PersistenceToUseGoesHere :
        PersistenceDefinition, IPersistenceDefinitionFactory<PersistenceToUseGoesHere>
    {
        static PersistenceToUseGoesHere IPersistenceDefinitionFactory<PersistenceToUseGoesHere>.Create() => new();
    }
}