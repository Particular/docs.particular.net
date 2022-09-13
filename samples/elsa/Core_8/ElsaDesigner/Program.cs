using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

public static class Program
{
  public static async Task Main(string[] args)
  {
    Console.Title = "Elsa Designer";

    var builder = WebApplication.CreateBuilder(args);

    ConfigureServices(builder.Services, builder.Configuration);

    var app = builder.Build();

    Configure(app);

    await app.RunAsync();

    void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
      var elsaSection = configuration.GetSection("Elsa");

      services
          .AddElsa(elsa => elsa
              .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
              .AddConsoleActivities()
              .AddHttpActivities(elsaSection.GetSection("Server").Bind)
              .AddQuartzTemporalActivities()
              .AddWorkflowsFrom<Startup>()
          );

      services.AddElsaApiEndpoints();
      services.AddRazorPages();
    }


  }

  static void Configure(IApplicationBuilder app)
  {
    app
        .UseStaticFiles()
        .UseHttpActivities()
        .UseRouting()
        .UseEndpoints(endpoints =>
        {
          endpoints.MapControllers();
          endpoints.MapFallbackToPage("/_Host");
        });
  }

}