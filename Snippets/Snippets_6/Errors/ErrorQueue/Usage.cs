namespace Snippets6.Errors.ErrorQueue
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ErrorWithCode
            endpointConfiguration.SendFailedMessagesTo("targetErrorQueue");

            #endregion
        }


    }
}