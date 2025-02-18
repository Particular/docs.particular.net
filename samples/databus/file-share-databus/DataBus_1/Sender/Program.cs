using NServiceBus;
using System;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.ClaimCheck.Sender");

        #region ConfigureDataBus

        var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
        claimCheck.BasePath(@"..\..\..\..\storage");

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press 'D' to send a large message");
        Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.N)
            {
                await SendMessageTooLargePayload(endpointInstance);
                continue;
            }

            if (key.Key == ConsoleKey.D)
            {
                await SendMessageLargePayload(endpointInstance);
                continue;
            }
            break;
        }
        await endpointInstance.Stop();
    }


    static async Task SendMessageLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageLargePayload

        var message = new MessageWithLargePayload
        {
            SomeProperty = "This message contains a large blob that will be sent on the claim check",
            LargeBlob = new ClaimCheckProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await endpointInstance.Send("Samples.ClaimCheck.Receiver", message);

        #endregion

        Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
    }

    static async Task SendMessageTooLargePayload(IEndpointInstance endpointInstance)
    {
        #region SendMessageTooLargePayload

        var message = new AnotherMessageWithLargePayload
        {
            LargeBlob = new byte[1024*1024*5] //5MB
        };
        await endpointInstance.Send("Samples.ClaimCheck.Receiver", message);

        #endregion
    }
}
