namespace Core.PolymorphicRouting;

using System;
using System.Threading.Tasks;
using NServiceBus;

#region PolymorphicRouting
public class SaveUser : IHandleMessages<UserCreated>
{
    public Task Handle(UserCreated message, IMessageHandlerContext context) {
        // Do stuff when the user is created
        return Task.CompletedTask;
    }
}

public class RecordCampaignActivity : IHandleMessages<CampaignActivityOccurred>
{
    public Task Handle(CampaignActivityOccurred message, IMessageHandlerContext context) {
        // Do stuff when an event related to a campaign happened
        return Task.CompletedTask;
    }
}
#endregion

#region PolymorphicRoutingMultipleInheritance
public interface UserCreated : IEvent
{
    Guid UserId { get; set; }
}

public interface CampaignActivityOccurred : IEvent
{
    Guid CampaignId { get; set; }
}

public interface UserCreatedFromCampaign : UserCreated, CampaignActivityOccurred
{
}
#endregion