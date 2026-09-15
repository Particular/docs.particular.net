namespace Core.Hosting;

using System.Text.Json.Serialization;
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

    void WriteDiagnosticsToLog(EndpointConfiguration endpointConfiguration)
    {
        #region WriteDiagnosticsToLog

        endpointConfiguration.WriteDiagnosticsToLog();

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

    void CustomDiagnosticsSection(IReadOnlySettings settings)
    {
        #region CustomDiagnosticsSection

        settings.AddStartupDiagnosticsSection(
            sectionName: "MySection",
            section: new MyDiagnostics
            {
                SomeSetting = "some data",
                SomeOtherSetting = 10
            },
            typeInfo: DiagnosticsJsonContext.Default.MyDiagnostics);

        #endregion
    }

    void CustomDiagnosticsSectionFactory(IReadOnlySettings settings)
    {
        #region CustomDiagnosticsSectionFactory

        settings.AddStartupDiagnosticsSectionFactory(
            sectionName: "MySection",
            sectionFactory: () => new MyDiagnostics
            {
                SomeSetting = "some data",
                SomeOtherSetting = 10
            },
            typeInfo: DiagnosticsJsonContext.Default.MyDiagnostics);

        #endregion
    }
}

#region CustomDiagnosticsSectionTypes

sealed class MyDiagnostics
{
    public required string SomeSetting { get; init; }
    public required int SomeOtherSetting { get; init; }
}

[JsonSerializable(typeof(MyDiagnostics))]
sealed partial class DiagnosticsJsonContext : JsonSerializerContext
{
}

#endregion
