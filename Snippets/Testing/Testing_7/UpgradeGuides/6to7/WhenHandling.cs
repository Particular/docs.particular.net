namespace Testing_7.UpgradeGuides._6to7
{
    using NServiceBus.Testing;
    using Testing_7.Saga;

    class WhenHandling
    {
        public void Run()
        {
            #region 6to7-WhenHandling

            var message = new CompleteSagaMessage();

            Test.Saga<MySaga>()
                .ExpectSagaCompleted()
                .WhenHandling(message);
            
            #endregion
        }

        class CompleteSagaMessage
        {
        }
    }
}
