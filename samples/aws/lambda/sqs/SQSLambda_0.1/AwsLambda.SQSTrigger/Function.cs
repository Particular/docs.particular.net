using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsLambda.SQSTrigger
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        static readonly AwsLambdaSQSEndpoint serverlessEndpoint = new AwsLambdaSQSEndpoint(context =>
        {
            var endpointConfiguration = new SQSTriggeredEndpointConfiguration("AwsLambdaSQSTrigger");
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
}
