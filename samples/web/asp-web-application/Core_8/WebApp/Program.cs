using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Program
{
    public static void Main()
    {
        #region ApplicationStart
        var builder = WebApplication.CreateBuilder();

        builder.Host.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.AsyncPages.WebApplication");
            endpointConfiguration.MakeInstanceUniquelyAddressable("1");
            endpointConfiguration.EnableCallbacks();
            endpointConfiguration.UseTransport(new LearningTransport());
            return endpointConfiguration;
        });
        #endregion

        builder.Services.AddRazorPages();

        var app = builder.Build();

        app.MapRazorPages();

        app.Run();
    }
}