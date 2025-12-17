using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

public class MySaga : Saga<RenewalSagaData>, IAmStartedByMessages<IRenewalSagaCommand>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<RenewalSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.PolicyHolderIdentifier)
            .ToMessage<IRenewalSagaCommand>(m => m.CustomerNumber);
    }
    public Task Handle(IRenewalSagaCommand message, IMessageHandlerContext context) => throw new NotImplementedException();
}

public class RenewalSagaData : CustomerBasedBaseSagaData{}

public abstract class CustomerBasedBaseSagaData : BaseSagaData
{
    public string PolicyHolderIdentifier { get; set; }
}

public abstract class BaseSagaData : ContainSagaData
{
    protected BaseSagaData()
    {
        SagaStartDate = DateTime.UtcNow;
        SagaLastModifiedDate = DateTime.UtcNow;
        PoliciesToHandle = new HashSet<string>();
    }
    public DateTime SagaStartDate { get; set; }
    public DateTime SagaLastModifiedDate { get; set; }
    public HashSet<string> PoliciesToHandle { get; set; }
    public bool TimeoutRequested { get; set; }
    public DateTime TimeoutTriggerTime { get; set; }
}
public interface IRenewalSagaCommand : ICustomerBasedBaseSagaCommand{}

public interface ICustomerBasedBaseSagaCommand : IBaseSagaCommand
{
    string CustomerNumber { get; set; }
}

public interface IBaseSagaCommand : ICommand
{
    string PolicyId { get; set; }
}