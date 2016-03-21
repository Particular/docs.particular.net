namespace Snippets6.Errors.ErrorQueue
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ErrorWithCode
            endpointConfiguration.SendFailedMessagesTo("targetErrorQueue");

            #endregion
        }
        
    }
}