using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Sender;
using Microsoft.Extensions.DependencyInjection;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
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
Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();