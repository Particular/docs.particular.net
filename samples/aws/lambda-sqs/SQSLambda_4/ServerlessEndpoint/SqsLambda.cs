using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

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
        static readonly IAwsLambdaSQSEndpoint serverlessEndpoint = Endpoint.Configuration;
    }
}