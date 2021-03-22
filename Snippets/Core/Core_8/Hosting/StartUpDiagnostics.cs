using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Settings;

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

        endpointConfiguration.CustomDiagnosticsWriter(
            (diagnostics, ct) =>
            {
                //custom logic to write data
                return Task.CompletedTask;
            });

        #endregion
    }

    void CustomDiagnosticsSection(ReadOnlySettings settings)
    {
        #region CustomDiagnosticsSection

        settings.AddStartupDiagnosticsSection(
            sectionName: "MySection",
            section: new
            {
                SomeSetting = "some data",
                SomeOtherSetting = 10
            });

        #endregion
    }
}