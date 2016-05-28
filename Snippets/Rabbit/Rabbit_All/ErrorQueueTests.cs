using NUnit.Framework;
using RabbitMQ.Client;

[TestFixture]
[Explicit]
public class ErrorQueueTests
{
    [Test]
    public void ReturnMessageToSourceQueue()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = AmqpTcpEndpoint.UseDefaultPort,
            UserName = "guest",
            Password = "guest",
        };
        using (var connection = connectionFactory.CreateConnection())
        {
            ErrorQueue.ReturnMessageToSourceQueue(
                brokerConnection: connection,
                errorQueueName: "error",
                messageId: "26afdee3-ac53-4990-95ef-a61300ca15c8");
        }
    }
}