namespace Snippets_6.Correlation
{
    using NServiceBus;

    public class CustomCorrelationId
    {
        public CustomCorrelationId()
        {
            IBus bus = null;

            #region custom-correlationid

            bus.Send("TargetQueue","My custom correlation id", new MyRequest());

            #endregion
        }

        public class MyRequest
        {
        }
    }
}