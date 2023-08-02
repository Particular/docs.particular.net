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

namespace LambdaFunctions
{
    public class Function
    {
        public Function()
        {
        }

        [LambdaFunction(Policies = "AWSLambda_FullAccess, AmazonSQSFullAccess")]
        [HttpApi(LambdaHttpMethod.Get, "/")]
        public async Task<string> HttpGetHandler(ILambdaContext context)
        {
            await serverlessEndpoint.Send(new TriggerMessage(), context);
            return $"{nameof(TriggerMessage)} sent.";
        }

        #region SQSEventFunctionHandler

        public async Task SqsHandler(SQSEvent evnt, ILambdaContext context)
        {
            using var cancellationTokenSource =
                new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

            await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
        }

        #endregion

        static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);

        #region EndpointSetup

        private static readonly IAwsLambdaSQSEndpoint serverlessEndpoint = new AwsLambdaSQSEndpoint(context =>
        {
            var endpointConfiguration = new AwsLambdaSQSEndpointConfiguration("ServerlessEndpoint");
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

             var routing = endpointConfiguration.RoutingSettings;

             routing.RouteToEndpoint(typeof(TriggerMessage), "ServerlessEndpoint");
             routing.RouteToEndpoint(typeof(ResponseMessage), "OnPremiseEndpoint");

            var advanced = endpointConfiguration.AdvancedConfiguration;
            advanced.SendFailedMessagesTo("error");

            return endpointConfiguration;
        });

        #endregion
    }
}