﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ASB.NativeIntegration.Sender";
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }

        var managementClient = NamespaceManager.CreateFromConnectionString(connectionString);
        var integrationQueue = "Samples.ASB.NativeIntegration";

        if(! await managementClient.QueueExistsAsync(integrationQueue)
            .ConfigureAwait(false))

        {
            await managementClient.CreateQueueAsync(integrationQueue)
                .ConfigureAwait(false);
        }

        var queueClient = QueueClient.CreateFromConnectionString(connectionString, integrationQueue);

        #region SerializedMessage

        var nativeMessage = @"{""Content"":""Hello from native sender"",""SentOnUtc"":""2015-10-27T20:47:27.4682716Z""}";

        #endregion

        var nativeMessageAsStream = new MemoryStream(Encoding.UTF8.GetBytes(nativeMessage));

        var message = new BrokeredMessage(nativeMessageAsStream)
        {
            MessageId = Guid.NewGuid().ToString()
        };

        #region NecessaryHeaders

        message.Properties["NServiceBus.EnclosedMessageTypes"] = "NativeMessage";
        message.Properties["NServiceBus.Transport.Encoding"] = "application/octect-stream";
        // Required to support ServiceControl that is using ASB v6.x
        message.Properties["NServiceBus.MessageIntent"] = "Send";

        #endregion

        await queueClient.SendAsync(message)
            .ConfigureAwait(false);

        Console.WriteLine("Native message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}