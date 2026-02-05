using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

// The namespace is required because the Lambda source generators expect it
namespace SQSLambdaSnippets;

class Usage(IAwsLambdaSQSEndpoint serverlessEndpoint)
{
    #region aws-endpoint-creation

    [LambdaStartup]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAwsLambdaSQSEndpoint("endpoint-name", (endpointConfiguration, _) =>
            {
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                //additional endpoint configuration
            });
        }
    }

    #endregion

    #region aws-function-definition

    [LambdaFunction]
    [SQSEvent("arn:aws:sqs:region:account:endpoint-name")]
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        using var cancellationTokenSource =
            new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

        await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
    }

    static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);

    #endregion

    static void CustomDiagnostics(AwsLambdaSQSEndpointConfiguration endpointConfiguration, ILambdaContext context)
    {
        #region aws-custom-diagnostics

        var advanced = endpointConfiguration.AdvancedConfiguration;

        advanced.CustomDiagnosticsWriter((diagnostics, token) =>
        {
            context.Logger.LogLine(diagnostics);
            return Task.CompletedTask;
        });

        #endregion
    }

    static void Recoverability(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-delayed-retries

        var recoverability = endpointConfiguration.AdvancedConfiguration.Recoverability();
        recoverability.Delayed(customization =>
        {
            customization.NumberOfRetries(5);
            customization.TimeIncrease(TimeSpan.FromSeconds(15));
        });

        #endregion
    }

    static void ConfigureDontMoveToErrors(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-configure-dont-move-to-error

        endpointConfiguration.DoNotSendMessagesToErrorQueue();

        #endregion
    }

    static void ConfigureSerializer(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-custom-serializer

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        #endregion
    }

    static void RoutingConfiguration(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-configure-routing

        var routing = endpointConfiguration.RoutingSettings;
        routing.RouteToEndpoint(typeof(ACommand), "<destination>");

        #endregion
    }

    class ACommand;
}
