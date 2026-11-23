namespace Testing_10.UpgradeGuides._10to11;

using System;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class SynchronousHandlerException
{
    #region 10to11-synchronous-handler-exception
    [Test]
    public void ThrowingHandlerExceptionIsWrapped()
    {
        var testableSaga = new TestableSaga<ThrowingSaga, ThrowingSagaData>();

        var exception = Assert.ThrowsAsync<TargetInvocationException>(
            async () => await testableSaga.Handle(new Start { CorrelationId = "abc" }));

        Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
    }
    #endregion

    public class ThrowingSaga : Saga<ThrowingSagaData>, IAmStartedByMessages<Start>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ThrowingSagaData> mapper) =>
            mapper.MapSaga(saga => saga.CorrelationId)
                .ToMessage<Start>(msg => msg.CorrelationId);

        // Not async, so the exception is thrown before a task is returned
        public Task Handle(Start message, IMessageHandlerContext context) =>
            throw new InvalidOperationException("Cannot start");
    }

    public class ThrowingSagaData : ContainSagaData
    {
        public string CorrelationId { get; set; }
    }

    public class Start : ICommand
    {
        public string CorrelationId { get; set; }
    }
}
