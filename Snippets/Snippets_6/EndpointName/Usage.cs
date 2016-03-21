namespace Snippets6.EndpointName
{
    using NServiceBus;

    class Usage
    {
        Usage()
        {
            #region EndpointNameCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("MyEndpoint");
            #endregion
        }

    }
}
