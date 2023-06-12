using System.Linq;
using System.Threading.Tasks;
using Common;
using Core8.Headers.Writers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NServiceBus;
using NUnit.Framework;

[TestFixture]
public class StartUpDiagnosticsWriter
{
    [Test]
    public async Task Write()
    {
        var endpointConfiguration = new EndpointConfiguration("StartUpDiagnosticsWriter");
        var typesToScan = TypeScanner.NestedTypes<HeaderWriterSend>();
        endpointConfiguration.SetTypesToScan(typesToScan);
        endpointConfiguration.UseTransport(new LearningTransport());
        string diagnostics = null;
        endpointConfiguration.CustomDiagnosticsWriter((x, ct) =>
        {
            diagnostics = x;
            return Task.CompletedTask;
        });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var jsonFormatted = JToken.Parse(diagnostics).ToString(Formatting.Indented);
        var substring = string.Join("\r", jsonFormatted.Split('\r').Take(20)) + "\r\n...";
        SnippetLogger.Write(substring);
        await endpointInstance.Stop();
    }
}