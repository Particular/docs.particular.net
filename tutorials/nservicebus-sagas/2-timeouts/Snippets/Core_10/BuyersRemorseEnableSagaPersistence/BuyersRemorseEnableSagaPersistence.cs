namespace BuyersRemorseEnableSagaPersistence;

class Program
{
    public void BuyersRemorseEnableSagaPersistence()
    {
        var endpointConfiguration = new EndpointConfiguration("Sales");

        #region BuyersRemorseEnableSagaPersistence
        var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
        #endregion
    }
}