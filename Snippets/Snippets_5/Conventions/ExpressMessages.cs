using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConvention 5

        var configuration = new BusConfiguration();

        configuration.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttribute 5
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}