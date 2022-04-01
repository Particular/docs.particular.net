using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

using Messages;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NServiceBus;
using NServiceBus.Activities;

using Sales;

using System;
using System.Threading.Tasks;

public static class Program
{
  public static async Task Main(string[] args)
  {
    Console.Title = "Sales";

    WebApplication app = null;
    var builder = WebApplication.CreateBuilder(args);

    ConfigureServices(builder.Services, builder.Configuration);

    builder.Host
        .UseNServiceBus(ctx =>
        {
          var endpointConfiguration = new EndpointConfiguration("Sales");

          var transport = endpointConfiguration.UseTransport<LearningTransport>();

          endpointConfiguration.Pipeline.Register(behavior: new CustomElsaHandlerTrigger(() => app.Services),
                                                  description: "Triggers listening Elsa workflows");

          return endpointConfiguration;
        });

    app = builder.Build();

    Configure(app);

    await app.RunAsync();
  }

  static void Configure(IApplicationBuilder app)
  {
    app
        .UseCors()
        .UseHttpActivities()
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
          endpoints.MapControllers();
        });
  }

  static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    var elsaSection = configuration.GetSection("Elsa");
    services
        .AddElsa(configure => configure
            .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
            .AddConsoleActivities()
            .AddHttpActivities(elsaSection.GetSection("Server").Bind)
            .AddNServiceBusActivities()
            .AddActivity<CreateOrderPlacedEvent>());
    services.AddNServiceBusBookmarkProviders();
    services.AddElsaApiEndpoints();

    services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("Content-Disposition"))
    );
  }
}
