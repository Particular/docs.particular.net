using NServiceBus.Testing;
using NServiceBus.UniformSession.Testing;

class TestSaga
{
    void TheTest()
    {
        #region UniformSessionSagaTesting
        var session = new TestableUniformSession();
        var saga = new SomeSaga(session);
        Test.Saga(saga)
            .WithUniformSession(session)
            .ExpectPublish<SomeEvent>()
            .WhenHandling<SomeCommand>();
        #endregion
    }
}