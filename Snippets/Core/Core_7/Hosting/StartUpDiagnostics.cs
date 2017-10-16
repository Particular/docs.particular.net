namespace Core7
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class StartUpDiagnostics
    {
        void SetDiagnosticsPath(EndpointConfiguration endpointConfiguration)
        {
            #region SetDiagnosticsPath

            endpointConfiguration.SetDiagnosticsPath("myCustomPath");

            #endregion
        }

        void CustomDiagnosticsWriter(EndpointConfiguration endpointConfiguration)
        {
            #region CustomDiagnosticsWriter

            endpointConfiguration.CustomDiagnosticsWriter(diagnostics =>
            {
                //custom logic to write data
                return Task.CompletedTask;
            });

            #endregion
        }
    }
}