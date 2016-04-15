namespace Snippets4.PubSub.WithEvent
{
    using NServiceBus;

    #region EventWithInterface

    namespace Domain.Messages
    {
        public class UserCreatedEvent : IEvent
        {
            public string Name { get; set; }
        }
    }

    #endregion
}
