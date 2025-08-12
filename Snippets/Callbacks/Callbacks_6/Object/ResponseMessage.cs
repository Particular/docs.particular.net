namespace Callbacks.Object
{
    using NServiceBus;

    #region CallbackResponseMessage
    public class ResponseMessage :
        IMessage
    {
        public string Property { get; set; }
    }
    #endregion
}