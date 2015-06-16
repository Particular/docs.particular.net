namespace Snippets5.Conventions.ExpressMessages
{
    using NServiceBus;

    #region ExpressMessageAttribute

    [Express]
    public class MyMessage : IMessage
    {
    }

    #endregion
}