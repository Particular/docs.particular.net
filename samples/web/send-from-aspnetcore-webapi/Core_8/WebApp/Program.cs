using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

public class Program
{
    public static Task Main()
    {
        #region EndpointConfiguration
        var builder = WebApplication.CreateBuilder();
        builder.Host.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
            var transport = endpointConfiguration.UseTransport(new LearningTransport());
            transport.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.ASPNETCore.Endpoint");

            endpointConfiguration.SendOnly();

            return endpointConfiguration;
        });
        #endregion

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        return app.RunAsync();
    }
}