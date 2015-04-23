using NServiceBus;

public class ExpressMessages
{
    public void Simple()
    {
        #region ExpressMEssageConvention

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.Conventions().DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));

        #endregion
    }

    #region ExpressMessageAttribute
    [Express]
    public class MyMessage : IMessage { }
    #endregion
}