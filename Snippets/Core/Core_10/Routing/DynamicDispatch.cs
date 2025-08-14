namespace Core.Routing;

using System;
using System.Threading.Tasks;
using NServiceBus;

#region DynamicDispatchHandlerStubs
public class SaveUser : IHandleMessages<UserCreated>
{
    public Task Handle(UserCreated message, IMessageHandlerContext context) {
        // Do stuff when the user is created
        return Task.CompletedTask;
    }
}

public class Audit : IHandleMessages<IMessage>
{
    public Task Handle(IMessage message, IMessageHandlerContext context) {
        // Audit the message
        return Task.CompletedTask;
    }
}
#endregion

#region DynamicDispatchEvolution
public class UserCreatedFromCampaign : UserCreated
{
    public Guid CampaignId { get; set; }
}

public class RecordStatistics : IHandleMessages<UserCreatedFromCampaign>
{
    public Task Handle(UserCreatedFromCampaign message, IMessageHandlerContext context) {
        // Record statistics
        return Task.CompletedTask;
    }
}
#endregion

public class UserCreated : IEvent { }