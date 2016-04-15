namespace Core5.PubSub.WithConvention
{

    #region EventWithConvention

    namespace Domain.Messages
    {
        public class UserCreatedEvent
        {
            public string Name { get; set; }
        }
    }

    #endregion
}
