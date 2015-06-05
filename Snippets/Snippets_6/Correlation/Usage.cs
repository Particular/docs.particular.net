namespace Snippets6.Correlation
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            IBus bus = null;

            #region custom-correlationid
            SendOptions options = new SendOptions();

            options.SetCorrelationId("My custom correlation id");

            bus.Send(new MyRequest(),options);

            #endregion
        }

    }
}