namespace Core.Sagas;

using System;
using System.Threading.Tasks;
using NServiceBus;

#region SagaAnalyzerComplexMapping
//todo: rework docs to not need this
#endregion
#region SagaAnalyzerSimplifiedMapping
//todo: rework docs to not need this
#endregion

public class ToStringMapping : Saga<SagaData>, IAmStartedByMessages<MessageWithString>, IAmStartedByMessages<MessageWithGuid>
{
    public Task Handle(MessageWithString message, IMessageHandlerContext context) => throw new NotImplementedException();
    public Task Handle(MessageWithGuid MessageWithGuid, IMessageHandlerContext context) => throw new NotImplementedException();

    #region SagaAnalyzerToMessageStringExpressions
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.StringValue)
            .ToMessage<MessageWithString>(msg => msg.StringValue)
            .ToMessage<MessageWithGuid>(msg => msg.GuidValue.ToString("N"));
    }
    #endregion
}

public class ToSagaToProperty : Saga<SagaData>, IAmStartedByMessages<MessageWithString>
{
    public Task Handle(MessageWithString message, IMessageHandlerContext context) => throw new NotImplementedException();
    public Task Handle(OrderBilled MessageWithGuid, IMessageHandlerContext context) => throw new NotImplementedException();

    #region SagaAnalyzerToMessageStringExpressions
    [System.Diagnostics.CodeAnalysis.SuppressMessage("NServiceBus.Sagas", "NSB0005:Saga can only define a single correlation property on the saga data", Justification = "Documenting invalid states")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("NServiceBus.Sagas", "NSB0017:ToSaga mapping must point to a property", Justification = "Documenting invalid states")]
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        #region SagaAnalyzerToSagaPropertyOk
        mapper.MapSaga(saga => saga.OrderId)
            #endregion
            .ToMessage<MessageWithString>(msg => msg.StringValue);

        #region SagaAnalyzerToSagaFieldNotOk
        mapper.MapSaga(saga => saga.SomeField) // INVALID
            #endregion
            .ToMessage<MessageWithString>(msg => msg.StringValue);

        #region SagaAnalyzerToSagaExpressionNotOk
        mapper.MapSaga(saga => saga.GuidValue.ToString()) // INVALID
            #endregion
            .ToMessage<MessageWithString>(msg => msg.StringValue);
    }
    #endregion
}

public class SagaData : ContainSagaData
{
    public string OrderId { get; set; }
    public string StringValue { get; set; }
    public string SomeField;
    public Guid GuidValue { get; set; }
}


public class OrderPlaced
{
    public string OrderId { get; set; }
}

public class OrderBilled
{
    public string OrderId { get; set; }
}

public class OrderShipped
{
    public string OrderId { get; set; }
}

public class MessageWithString
{
    public string StringValue { get; set; }
}

public class MessageWithGuid
{
    public Guid GuidValue { get; set; }
}