using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConventionV5

        var configuration = new BusConfiguration();

        configuration.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttributeV5
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}