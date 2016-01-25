namespace Snippets5.Correlation
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            IBus bus = null;

            #region custom-correlationid

            bus.Send("TargetQueue", "My custom correlation id", new MyRequest());

            #endregion
        }

    }
}