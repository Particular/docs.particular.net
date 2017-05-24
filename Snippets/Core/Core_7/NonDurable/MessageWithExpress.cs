namespace Core7.NonDurable
{
    using NServiceBus;

    #region ExpressMessageAttribute

    [Express]
    public class MessageWithExpress :
        IMessage
    {
    }

    #endregion
}