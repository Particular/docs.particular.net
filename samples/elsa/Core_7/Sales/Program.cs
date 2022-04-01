using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

using Messages;

using NServiceBus;
using NServiceBus.Activities;

using Sales;

Console.Title = "Sales";

WebApplication? app = null;
var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

builder.Host
    .UseNServiceBus(ctx =>
    {
        var endpointConfiguration = new EndpointConfiguration("Sales");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();

        endpointConfiguration.Pipeline.Register(behavior: new CustomElsaHandlerTrigger(() => GetServiceProvider()),
                                                description: "Triggers listening Elsa workflows");

        return endpointConfiguration;
    });

app = builder.Build();

Configure(app);

await app.RunAsync();

IServiceProvider? GetServiceProvider()
{
    return app?.Services;
}

void Configure(IApplicationBuilder app)
{
    app
        .UseCors()
        .UseHttpActivities()
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
            // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
            endpoints.MapControllers();
        });
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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

    // Allow arbitrary client browser apps to access the API.
    // In a production environment, make sure to allow only origins you trust.
    services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("Content-Disposition"))
    );
}

