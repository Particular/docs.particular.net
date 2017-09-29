namespace Testing_7.UpgradeGuides._6to7
{
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Testing_6.Saga;

    [Explicit]
    [TestFixture]
    public class ExpectSagaCompleted
    {
        [Test]
        public void Run()
        {
            #region 6to7-ExpectSagaCompleted

            Test.Saga<MySaga>()
                .WhenHandling<CompleteSagaMessage>()
                .AssertSagaCompletionIs(true);

            #endregion
        }

        public class CompleteSagaMessage
        {
        }
    }
}