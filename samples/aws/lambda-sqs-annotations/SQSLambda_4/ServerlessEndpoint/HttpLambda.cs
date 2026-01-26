using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace LambdaFunctions;

public class HttpLambda(IAwsLambdaSQSEndpoint serverlessEndpoint)
{
    #region HttpFunctionHandler
    [LambdaFunction(Policies = "AWSLambda_FullAccess, AmazonSQSFullAccess")]
    [HttpApi(LambdaHttpMethod.Get, "/")]
    public async Task<string> HttpGetHandler(ILambdaContext context)
    {
        await serverlessEndpoint.Send(new TriggerMessage(), context);
        return $"{nameof(TriggerMessage)} sent.";
    }
    #endregion
}