using NServiceBus;
using NServiceBus.ClaimCheck;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Title = "Receiver";
        var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Receiver");
        
        var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
        claimCheck.BasePath(@"..\..\..\..\storage");
        
        // Configure ClaimCheck properties using conventions
        endpointConfiguration.Conventions()
            .DefiningClaimCheckPropertiesAs(property => property.Name.EndsWith("Blob"));
        
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}