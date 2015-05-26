namespace Snippets_6.Correlation
{
    using NServiceBus;

    public class CustomCorrelationId
    {
        public CustomCorrelationId()
        {
            IBus bus = null;

            #region custom-correlationid
            SendOptions options = new SendOptions();

            options.SetCorrelationId("My custom correlation id");

            bus.Send(new MyRequest(),options);

            #endregion
        }

        public class MyRequest
        {
        }
    }
}