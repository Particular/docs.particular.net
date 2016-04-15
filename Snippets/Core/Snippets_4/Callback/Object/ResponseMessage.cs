using NServiceBus;

namespace Core4.Callback.Object
{
    #region CallbackResponseMessage
    public class ResponseMessage : IMessage
    {
        public string Property { get; set; }
    }
    #endregion
}
