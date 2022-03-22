using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region LearningPersistence

        endpointConfiguration.UsePersistence<LearningPersistence>();

        #endregion
    }

    void SagaStorageDirectory(EndpointConfiguration endpointConfiguration)
    {
        #region LearningPersistenceSagaStorageDirectory

        var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
        persistence.SagaStorageDirectory("PathToStoreSagas");

        #endregion
    }
}