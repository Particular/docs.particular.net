using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

namespace LambdaFunctions;

public class SqsLambda(IAwsLambdaSQSEndpoint serverlessEndpoint)
{
    #region SqsFunctionHandler
    [LambdaFunction]
    [SQSEvent("arn:aws:sqs:region:account-id:ServerlessEndpoint")]
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        using var cancellationTokenSource =
            new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

        await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
    }
    #endregion

    static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);
}