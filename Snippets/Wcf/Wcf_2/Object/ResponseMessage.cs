namespace Wcf1.Object
{
    using NServiceBus;

    #region WcfCallbackResponseMessage

    public class ResponseMessage :
        IMessage
    {
        public string Property { get; set; }
    }

    #endregion
}