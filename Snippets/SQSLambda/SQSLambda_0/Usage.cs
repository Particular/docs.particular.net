using System.Threading.Tasks;
using NServiceBus;

class Usage
{
    #region endpoint-configuration

    static readonly AwsLambdaSQSEndpoint lambdaEndpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("AwsLambdaSQSTrigger");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        var advanced = endpointConfiguration.AdvancedConfiguration;
        advanced.SendFailedMessagesTo("ErrorAwsLambdaSQSTrigger");

        // shows how to write diagnostics to file
        advanced.CustomDiagnosticsWriter(diagnostics =>
        {
            context.Logger.LogLine(diagnostics);
            return Task.CompletedTask;
        });

        return endpointConfiguration;
    });

    #endregion
}