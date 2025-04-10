using System.Threading.Tasks;
using Amazon.Lambda.SQSEvents;
using NServiceBus;

#region native-lambda-sqs-message

public class AccessToLambdaNativeMessageInHandler : IHandleMessages<TestMessage>
{
  public Task Handle(TestMessage message, IMessageHandlerContext context)
  {
    var nativeMessage = context.Extensions.Get<SQSEvent.SQSMessage>();

    return Task.CompletedTask;
  }
}

#endregion

public class TestMessage;