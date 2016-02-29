namespace Snippets6.Errors.ErrorQueue
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ErrorWithCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.SendFailedMessagesTo("targetErrorQueue");

            #endregion
        }


    }
}