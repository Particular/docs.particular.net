namespace Core8.DynamicDispatch
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region DynamicDispatchHandlerStubs
    public class SaveUser : IHandleMessages<UserCreated>
    {
        public async Task Handle(UserCreated message, IMessageHandlerContext) {
            // Do stuff when the user is created
        }
    }

    public class Audit : IHandleMessages<IMessage>
    {
        public async Task Handle(IMessage message, IMessageHandlerContext context) {
            // Audit the message
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
        public async Task Handle(UserCreatedFromCampaign, IMessageHandlerContext context) {
            // Record statistics
        }
    }
    #endregion

    public class UserCreated : IEvent { }

}