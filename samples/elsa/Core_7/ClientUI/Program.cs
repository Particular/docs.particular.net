using ClientUI;

using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

using NServiceBus;
using NServiceBus.Activities;

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
        // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
        endpoints.MapControllers();
    });

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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

    // Allow arbitrary client browser apps to access the API.
    // In a production environment, make sure to allow only origins you trust.
    // These API's are used by the Elsa designer to push workflows at runtime.
    services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("Content-Disposition"))
    );
}

