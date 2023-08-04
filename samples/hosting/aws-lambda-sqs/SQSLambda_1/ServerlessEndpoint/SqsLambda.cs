using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

namespace LambdaFunctions
{
    public class SqsLambda
    {
        #region SqsFunctionHandler
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            using var cancellationTokenSource =
                new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

            await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
        }
        #endregion

        static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);

        private static readonly IAwsLambdaSQSEndpoint serverlessEndpoint = EndpointConfiguration.Configure();


    }
}