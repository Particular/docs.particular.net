using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NServiceBus;
using NServiceBus.Persistence;

Console.Title = "Server";

#region Config

var endpointConfiguration = new EndpointConfiguration("Samples.NHibernate.Server");
var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=Samples.NHibernate;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=Samples.NHibernate;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
var hibernateConfig = new Configuration();
hibernateConfig.DataBaseIntegration(x =>
{
    x.ConnectionString = connectionString;
    x.Dialect<MsSql2012Dialect>();
    x.Driver<MicrosoftDataSqlClientDriver>();
});

AddMappings(hibernateConfig);

persistence.UseConfiguration(hibernateConfig);

#endregion

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();


static void AddMappings(Configuration nhConfiguration)
{
    var mapper = new ModelMapper();
    mapper.AddMappings(typeof(OrderShipped).Assembly.GetTypes());
    nhConfiguration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
}