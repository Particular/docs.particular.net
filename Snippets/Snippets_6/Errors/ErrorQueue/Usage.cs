namespace Snippets6.Errors.ErrorQueue
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ErrorWithCode

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.SendFailedMessagesTo("targetErrorQueue");

            #endregion
        }


    }
}