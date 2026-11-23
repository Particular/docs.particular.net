using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Usage
{
    public void Ctor()
    {
        #region TestableSagaCtor
        // For testing: public class MySaga : Saga<MyData>
        var testableSaga = new TestableSaga<MySaga, MyData>();
        #endregion
    }

    public void CtorFactory()
    {
        #region TestableSagaCtorFactory
        var testableSaga = new TestableSaga<MySaga, MyData>(
            sagaFactory: () => new MySaga(new InjectedService()));
        #endregion
    }

    public void CtorTime()
    {
        #region TestableSagaCtorTime
        var testableSaga = new TestableSaga<MySaga, MyData>(
            initialCurrentTime: new DateTime(2022, 01, 01, 0, 0, 0, DateTimeKind.Utc));
        #endregion
    }

    public async Task Defaults(TestableSaga<MySaga, MyData> testableSaga)
    {
        #region TestableSagaSimpleHandle
        var handleResult = await testableSaga.Handle(new MyCommand());
        #endregion

        var timeSpanToAdvance = TimeSpan.FromHours(1);

        #region TestableSagaAdvanceTime
        // Returns HandleResult[]
        var timeoutResults = await testableSaga.AdvanceTime(timeSpanToAdvance);

        DateTime newCurrentTime = testableSaga.CurrentTime;
        #endregion

        #region TestableSagaSimulateReply
        testableSaga.SimulateReply<DoStep1, Step1Response>(step1Command =>
        {
            return new Step1Response();
        });
        #endregion

        var previousHandleResult = handleResult;

        #region TestableSagaHandleReply
        await testableSaga.HandleReply(previousHandleResult.SagaId, new Step1Response());
        #endregion

        #region TestableSagaHandleReplyParams
        var customHandlerContext = new TestableMessageHandlerContext();
        var customHeaders = new Dictionary<string, string>();

        await testableSaga.HandleReply(previousHandleResult.SagaId, new Step1Response(),
            context: customHandlerContext,
            messageHeaders: customHeaders);
        #endregion

        #region TestableSagaQueueOperations
        bool hasMessages = testableSaga.HasQueuedMessages;
        int size = testableSaga.QueueLength;
        var nextMessage = testableSaga.QueuePeek();
        #endregion

        #region TestableSagaHandleQueuedMessage
        await testableSaga.HandleQueuedMessage();

        var customContext = new TestableMessageHandlerContext();
        await testableSaga.HandleQueuedMessage(context: customContext);
        #endregion
    }

    public async Task AllParams(TestableSaga<MySaga, MyData> testableSaga)
    {
        #region TestableSagaHandleParams
        var customHandlerContext = new TestableMessageHandlerContext();
        var customHeaders = new Dictionary<string, string>();

        var handleResult = await testableSaga.Handle(new MyCommand(), context: customHandlerContext, messageHeaders: customHeaders);
        #endregion

        var timeSpanToAdvance = TimeSpan.FromHours(1);

        #region TestableSagaAdvanceTimeParams
        // Returns HandleResult[]
        var timeoutResults = await testableSaga.AdvanceTime(timeSpanToAdvance,
            provideContext: timeoutDetails =>
            {
                // Provide custom context based on timeout details
                return new TestableMessageHandlerContext();
            });
        #endregion
    }

    public class InjectedService { }

    public class MySaga : NServiceBus.Saga<MyData>
    {
        public MySaga() { }
        public MySaga(InjectedService service) { }
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MyData> mapper)
        {

        }
    }

    public class MyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool Placed { get; set; }
        public bool Billed { get; set; }
    }

    public class MyCommand : ICommand
    {
        public string CorrelationId { get; set; }
    }

    public class DoStep1 : ICommand { }
    public class Step1Response : IMessage { }
}