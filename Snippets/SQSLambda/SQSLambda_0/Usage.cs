using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

class Usage
{
    #region aws-endpoint-creation

    static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("AwsLambdaSQSTrigger");
        
        //customize configuration here

        return endpointConfiguration;
    });

    #endregion

    #region aws-function-definition

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        var cancellationDelay = context.RemainingTime.Subtract(TimeSpan.FromSeconds(10));
        using (var cancellationTokenSource = new CancellationTokenSource(cancellationDelay))
        {
            await endpoint.Process(evnt, context, cancellationTokenSource.Token);
        }
    }

    #endregion

    static void CustomDiagnostics(AwsLambdaSQSEndpointConfiguration endpointConfiguration, ILambdaContext context)
    {
        #region aws-custom-diagnostics

        var advanced = endpointConfiguration.AdvancedConfiguration;

        advanced.CustomDiagnosticsWriter(diagnostics =>
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

        #region aws-unrestricted-delayed-delivery

        endpointConfiguration.Transport.UnrestrictedDurationDelayedDelivery();

        #endregion
    }

    static void ConfigureErrorQueue(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-configure-error-queue

        var advanced = endpointConfiguration.AdvancedConfiguration;

        advanced.SendFailedMessagesTo("<error-queue>");

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

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        
        #endregion
    }

    static void RoutingConfiguration(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region aws-configure-routing

        var transport = endpointConfiguration.Transport;
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(ACommand), "<destination>");
        
        #endregion
    }

    class ACommand { }
}
