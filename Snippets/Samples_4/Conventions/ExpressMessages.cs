using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConventionV4

        var configure = Configure.With()
            .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttributeV4
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}