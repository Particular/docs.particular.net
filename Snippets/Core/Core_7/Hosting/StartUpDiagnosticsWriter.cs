using System.Threading;
using System.Threading.Tasks;
using Common;
using Core7.Headers.Writers;
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
        endpointConfiguration.UseTransport<LearningTransport>();
        string diagnostics = null;
        endpointConfiguration.CustomDiagnosticsWriter(x =>
        {
             diagnostics = x;
            return Task.CompletedTask;
        });

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        SnippetLogger.Write(diagnostics);
        await endpointInstance.Stop();
    }
}