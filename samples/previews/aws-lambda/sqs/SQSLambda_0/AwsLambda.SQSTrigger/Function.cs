using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

public class Function
{
    public Function()
    {
    }

    #region FunctionHandler
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        using (var cancellationTokenSource = new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod)))
        {
            await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
        }
    }
    #endregion

    static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);

    #region EndpointSetup

    private static readonly IAwsLambdaSQSEndpoint serverlessEndpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("AwsLambdaSQSTrigger");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        var transport = endpointConfiguration.Transport;
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(BackToSenderMessage), "AwsLambda.Sender");

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
