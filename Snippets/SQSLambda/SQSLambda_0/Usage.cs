using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

class Usage
{
    #region endpoint-creation

    static readonly AwsLambdaSQSEndpoint endpoint = new AwsLambdaSQSEndpoint(context =>
    {
        var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("AwsLambdaSQSTrigger");
        
        //customize configuration here

        return endpointConfiguration;
    });

    #endregion

    #region function-definition

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
        #region custom-diagnostics

        var advanced = endpointConfiguration.AdvancedConfiguration;

        advanced.CustomDiagnosticsWriter(diagnostics =>
        {
            context.Logger.LogLine(diagnostics);
            return Task.CompletedTask;
        });

        #endregion
    }

    static void LicenseFile(AwsLambdaSQSEndpointConfiguration endpointConfiguration, ILambdaContext context)
    {
        #region load-license-file

        var licenseText = Environment.GetEnvironmentVariable("NServiceBusLicenseText");

        var advanced = endpointConfiguration.AdvancedConfiguration;
        
        advanced.License(licenseText);

        #endregion
    }

    static void Recoverability(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region delayed-retries

        var recoverability = endpointConfiguration.AdvancedConfiguration.Recoverability();
        recoverability.Delayed(customization =>
        {
            customization.NumberOfRetries(5);
            customization.TimeIncrease(TimeSpan.FromSeconds(15));
        });

        #endregion

        #region unrestricted-delayed-delivery

        endpointConfiguration.Transport.UnrestrictedDurationDelayedDelivery();

        #endregion
    }

    static void ConfigureErrorQueue(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region configure-error-queue

        var advanced = endpointConfiguration.AdvancedConfiguration;

        advanced.SendFailedMessagesTo("<error-queue>");

        #endregion
    }

    static void ConfigureDontMoveToErrors(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region configure-dont-move-to-error

        endpointConfiguration.DoNotSendMessagesToErrorQueue();

        #endregion
    }
    static void ConfigureSerializer(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region custom-serializer

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        
        #endregion
    }

    static void RoutingConfiguration(AwsLambdaSQSEndpointConfiguration endpointConfiguration)
    {
        #region configure-routing

        var transport = endpointConfiguration.Transport;
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(ACommand), "<destination>");
        
        #endregion
    }

    class ACommand { }
}