using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NServiceBus.Persistence;
using Sample;



var endpointName = "Samples.NHibernateCustomSagaFinder";
Console.Title = endpointName;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region config

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesNhCustomSagaFinder;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesNhCustomSagaFinder;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var hibernateConfig = new Configuration();
hibernateConfig.DataBaseIntegration(x =>
{
    x.ConnectionString = connectionString;
    x.Dialect<MsSql2012Dialect>();
    x.Driver<MicrosoftDataSqlClientDriver>();
});

var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>()
    .UseConfiguration(hibernateConfig);

#endregion

SqlHelper.EnsureDatabaseExists(connectionString);

Console.WriteLine("Press any key to exit");
Console.ReadKey();


builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<InputLoopService>();
await builder.Build().RunAsync();