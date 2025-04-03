using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

namespace LambdaFunctions;

public class SqsLambda(AwsLambdaSQSEndpoint serverlessEndpoint)
{
    #region SqsFunctionHandler
    [LambdaFunction]
    [SQSEvent("arn:aws:sqs:eu-south-1:714232833252:ServerlessEndpoint")]
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        using var cancellationTokenSource =
            new CancellationTokenSource(context.RemainingTime.Subtract(DefaultRemainingTimeGracePeriod));

        await serverlessEndpoint.Process(evnt, context, cancellationTokenSource.Token);
    }
    #endregion

    static readonly TimeSpan DefaultRemainingTimeGracePeriod = TimeSpan.FromSeconds(10);
}