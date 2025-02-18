using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;

class Program
{
    static async Task Main()
    {
        Console.Title = "Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

        #region ConfigureDataBus

        var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
        claimCheck.BasePath(@"..\..\..\..\storage");

        #endregion

        #region CustomJsonSerializerOptions
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new ClaimCheckPropertyConverterFactory());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);
        #endregion

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press 'D' to send a databus large message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

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
            SomeProperty = "This message contains a large blob that will be sent on the data bus",
            LargeBlob = new ClaimCheckProperty<byte[]>(new byte[1024*1024*5]) //5MB
        };
        await endpointInstance.Send("Samples.DataBus.Receiver", message);

        #endregion

        Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
    }
}
