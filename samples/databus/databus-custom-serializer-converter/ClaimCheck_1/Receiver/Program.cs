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
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
        claimCheck.BasePath(@"..\..\..\..\storage");

        //CustomJsonSerializerOptions
        var jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(new ClaimCheckPropertyConverterFactory());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);

        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}
