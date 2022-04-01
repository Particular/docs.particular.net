using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

Console.Title = "Elsa Designer";

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Configure(app);

await app.RunAsync();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var elsaSection = configuration.GetSection("Elsa");

    // Elsa services.
    services
        .AddElsa(elsa => elsa
            .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
            .AddConsoleActivities()
            .AddHttpActivities(elsaSection.GetSection("Server").Bind)
            .AddQuartzTemporalActivities()
            .AddWorkflowsFrom<Startup>()
        );

    // Elsa API endpoints.
    services.AddElsaApiEndpoints();

    // For Dashboard.
    services.AddRazorPages();
}

void Configure(IApplicationBuilder app)
{
    app
        .UseStaticFiles() // For Dashboard.
        .UseHttpActivities()
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
            // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
            endpoints.MapControllers();

            // For Dashboard.
            endpoints.MapFallbackToPage("/_Host");
        });
}