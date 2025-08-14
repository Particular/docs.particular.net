namespace Core.AssemblyScanning;

using NServiceBus;

public class DisableAssemblyFileScanning
{
    public void DisableFileScanning(EndpointConfiguration endpointConfiguration)
    {
        #region disable-file-scanning
        endpointConfiguration.AssemblyScanner().ScanFileSystemAssemblies = false;
        #endregion
    }
}