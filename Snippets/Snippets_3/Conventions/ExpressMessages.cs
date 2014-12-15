using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConvention

        var configure = Configure.With()
            .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttribute
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}