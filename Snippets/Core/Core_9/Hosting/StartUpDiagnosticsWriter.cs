using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Core.Headers.Writers;
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
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        var jsonFormatted = JToken.Parse(diagnostics).ToString(Formatting.Indented);
        var abbreviated = jsonFormatted.Split(["\r\n", "\n", "\r"], StringSplitOptions.None).Take(20);
        var substring = string.Join("\r", abbreviated) + "\r\n...";
        SnippetLogger.Write(substring);
        await endpointInstance.Stop();
    }
}