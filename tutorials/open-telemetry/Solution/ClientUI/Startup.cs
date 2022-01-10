using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientUI
{
    using Azure.Monitor.OpenTelemetry.Exporter;
    using Honeycomb.OpenTelemetry;
    using OpenTelemetry.Resources;
    using OpenTelemetry.Trace;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc();

            services.AddOpenTelemetryTracing(builder => builder
                                                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Program.EndpointName))
                                                        //.AddHttpClientInstrumentation()
                                                        .AddAspNetCoreInstrumentation(opt => opt.Enrich = (activity, key, value) =>
                                                        {
                                                            Console.WriteLine($"Got an activity named {key}");
                                                        })
                                                        .AddSource("NServiceBus")
                                                        .AddJaegerExporter(c =>
                                                        {
                                                            c.AgentHost = "localhost";
                                                            c.AgentPort = 6831;
                                                        })
                                                        .AddAzureMonitorTraceExporter(c => { c.ConnectionString = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY"); })
                                                        .AddHoneycomb(new HoneycombOptions
                                                        {
                                                            ServiceName = "spike",
                                                            ApiKey = Environment.GetEnvironmentVariable("HONEYCOMB_APIKEY"),
                                                            Dataset = "spike-core"
                                                        })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
