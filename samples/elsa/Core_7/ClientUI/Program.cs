using ClientUI;

using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NServiceBus;
using NServiceBus.Activities;

using System;
using System.Threading.Tasks;

public static class Program
{
  public static async Task Main(string[] args)
  {
    Console.Title = "ClientUI";

    var builder = WebApplication.CreateBuilder(args);

    ConfigureServices(builder.Services, builder.Configuration);

    builder.Host
        .UseNServiceBus(ctx =>
        {
          var endpointConfiguration = new EndpointConfiguration("ClientUI");
          var transport = endpointConfiguration.UseTransport<LearningTransport>();
          return endpointConfiguration;
        });

    var app = builder.Build();

    app
        .UseCors()
        .UseHttpActivities()
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
          endpoints.MapControllers();
        });

    await app.RunAsync();
  }

  static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    var elsaSection = configuration.GetSection("Elsa");

    services
        .AddElsa(configure => configure
            .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
            .AddConsoleActivities()
            .AddQuartzTemporalActivities()
            .AddHttpActivities(elsaSection.GetSection("Server").Bind)
            .AddActivity<CreatePlaceOrderMessage>()
            .AddNServiceBusActivities());
    services.AddElsaApiEndpoints();

    services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("Content-Disposition"))
    );
  }
}

