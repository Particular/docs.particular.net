#pragma warning disable 618
namespace Core7.Scanning
{
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningNestedAssembliesEnabled(EndpointConfiguration endpointConfiguration)
        {
            var additionalPathToScanAssemblies = "";

            #region AdditionalAssemblyScanningPath

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.AdditionalAssemblyScanningPath = additionalPathToScanAssemblies;

            #endregion
        }
    }
}
