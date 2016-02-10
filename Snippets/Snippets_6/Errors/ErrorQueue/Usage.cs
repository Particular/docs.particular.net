namespace Snippets6.Errors.ErrorQueue
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region ErrorWithCode

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.SendFailedMessagesTo("targetErrorQueue");

            #endregion
        }


    }
}