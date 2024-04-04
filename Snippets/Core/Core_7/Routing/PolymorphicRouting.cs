namespace Core7.PolymorphicRouting
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region PolymorphicRouting
    public class SaveUser : IHandleMessages<UserCreated>
    {
        public async Task Handle(UserCreated message, IMessageHandlerContext) {
            // Do stuff when the user is created
        }
    }

    public class RecordCampaignActivity : IHandleMessages<CampaignActivityOccurred>
    {
        public async Task Handle(CampaignActivityOccurred message, IMessageHandlerContext) {
            // Do stuff when an event related to a campaign happened
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
}