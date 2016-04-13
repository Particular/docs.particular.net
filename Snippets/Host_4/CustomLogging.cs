namespace Snippets4.Host
{
    using NServiceBus;

    #region CustomHostLogging
    class MyEndpointConfig : IConfigureThisEndpoint, IWantCustomLogging
    {
        public void Init()
        {
            // setup your logging infrastructure then call
            SetLoggingLibrary.Log4Net(null, new MyLogger());
        }
    }
    #endregion

    class MyLogger
    {
    }
}