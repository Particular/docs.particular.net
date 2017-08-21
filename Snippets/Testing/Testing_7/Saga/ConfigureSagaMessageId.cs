namespace Testing_6.Saga
{
    using NServiceBus.Testing;

    public class ConfigureSagaMessageId
    {
        public void Run()
        {
            #region ConfigureSagaMessageId
            Test.Saga<MySaga>()
                .ConfigureHandlerContext(c =>
                {
                    c.MessageId = "my message ID";
                })
                .ExpectPublish<MyEvent>()
                .When(s => s.Handle, new StartsSaga());
            #endregion
        }
    }
}