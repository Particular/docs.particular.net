using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NServiceBus;
using NServiceBus.Persistence;

class Program
{
    static async Task Main()
    {
        Console.Title = "Default";

        var endpointConfiguration = new EndpointConfiguration("Samples.CustomNhMappings.Default");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=Samples.CustomNhMappings;Integrated Security=True;Max Pool Size=100;Encrypt=false
        var connectionString = @"Server=localhost,1433;Initial Catalog=Samples.CustomNhMappings;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
        var hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionString = connectionString;
            x.Dialect<MsSql2012Dialect>();
            x.Driver<MicrosoftDataSqlClientDriver>();
        });

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(hibernateConfig);

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

        var startOrder = new StartOrder
        {
            OrderId = "123"
        };
        await messageSession.SendLocal(startOrder);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await host.StopAsync();
    }
}
