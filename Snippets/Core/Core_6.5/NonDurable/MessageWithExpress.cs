namespace Core6.NonDurable
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