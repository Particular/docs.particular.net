using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

public static class RabbitHelper
{
    #region WaitForRabbit

    public static async Task WaitForRabbitToStart()
    {
        var factory = new ConnectionFactory
        {
            Uri = "amqp://rabbitmq"
        };
        for (var i = 0; i < 5; i++)
        {
            try
            {
                using (factory.CreateConnection())
                {
                }
                return;
            }
            catch (BrokerUnreachableException)
            {
            }
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }

    #endregion
}