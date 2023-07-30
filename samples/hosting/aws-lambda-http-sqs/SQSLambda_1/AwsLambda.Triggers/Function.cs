using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

using NServiceBus;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Triggers
{
    public class Function
    {
        static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);

        public Function()
        {
        }

        #region AwsLambda-HttpFunctionHandler
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/")]
        public async Task<string> HttpGet(ILambdaContext context)
        {
            await serverlessEndpoint.Send(new SQSTriggerMessage(), context);
            return $"{nameof(SQSTriggerMessage)} sent.";
        }
        #endregion

        #region AwsLambda-SQSFunctionHandler
        public async Task SQSFunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            using var cancellationTokenSource = new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

            await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
        }
        #endregion

        #region NServiceBus-AwsLambdaSQS-EndpointSetup
        static readonly IAwsLambdaSQSEndpoint serverlessEndpoint = new AwsLambdaSQSEndpoint(context =>
        {
            var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("AwsLambdaSQSTrigger");
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

            var routing = endpointConfiguration.RoutingSettings;

            routing.RouteToEndpoint(typeof(SQSTriggerMessage), "AwsLambdaSQSTrigger");
            routing.RouteToEndpoint(typeof(BackToSenderMessage), "AwsLambda.Sender");

            var advanced = endpointConfiguration.AdvancedConfiguration;
            advanced.SendFailedMessagesTo("Error");
            //advanced.SendFailedMessagesTo("ErrorAwsLambdaSQSTrigger");

            return endpointConfiguration;
        });
        #endregion
    }
}