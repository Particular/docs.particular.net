using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConvention 4

        var configure = Configure.With()
            .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttribute 4
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}