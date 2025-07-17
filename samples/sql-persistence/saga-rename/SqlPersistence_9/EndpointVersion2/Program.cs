using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.MessageMutator;
using Shared;

Console.Title = "EndpointVersion2";

// Cnfigure the endpoint
var endpointConfiguration = new EndpointConfiguration("Samples.RenameSaga");
SharedConfiguration.Apply(endpointConfiguration);

#region registerMutator

endpointConfiguration.RegisterMessageMutator(new EndpointVersion2.ReplyMutator());

#endregion

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

// Get required services
var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(@"Renaming SQL tables:
    from Samples_RenameSaga_MyReplySagaVersion1 to Samples_RenameSaga_MyReplySagaVersion2
    from Samples_RenameSaga_MyTimeoutSagaVersion1 to Samples_RenameSaga_MyTimeoutSagaVersion2");

#region renameTables

// for SqlExpress: 
//var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;Integrated Security=True;Encrypt=false";
// for SQL Server:
//var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";
// for LocalDB:
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

using var renameReplySaga = connection.CreateCommand();
renameReplySaga.CommandText = "exec sp_rename 'Samples_RenameSaga_MyReplySagaVersion1', 'Samples_RenameSaga_MyReplySagaVersion2'";
await renameReplySaga.ExecuteNonQueryAsync();

using var renameTimeoutSaga = connection.CreateCommand();
renameTimeoutSaga.CommandText = "exec sp_rename 'Samples_RenameSaga_MyTimeoutSagaVersion1', 'Samples_RenameSaga_MyTimeoutSagaVersion2'";
await renameTimeoutSaga.ExecuteNonQueryAsync();

#endregion

// Start the host
await host.StartAsync();

Console.WriteLine("Waiting to receive timeout and reply. Should happen within 10 seconds");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

// Stop the host
await host.StopAsync();
