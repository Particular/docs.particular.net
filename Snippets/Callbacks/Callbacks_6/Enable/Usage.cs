namespace Callbacks.Enable
{
    using NServiceBus;

    class Usage
    {
        void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region EnableCallbacks-Default

            endpointConfiguration.EnableCallbacks();

            #endregion

            #region EnableCallbacks-NoRequests

            endpointConfiguration.EnableCallbacks(makesRequests: false);

            #endregion
        }
    }
}