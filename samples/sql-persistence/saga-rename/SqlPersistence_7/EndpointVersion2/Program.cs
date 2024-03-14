using System;
using Microsoft.Data.SqlClient;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;


var defaultFactory = LogManager.Use<DefaultFactory>();
defaultFactory.Level(LogLevel.Warn);

Console.Title = "Samples.RenameSaga.Version2";

Console.WriteLine("Renaming SQL tables:");
Console.WriteLine("    from Samples_RenameSaga_MyReplySagaVersion1 to Samples_RenameSaga_MyReplySagaVersion2");
Console.WriteLine("    from Samples_RenameSaga_MyTimeoutSagaVersion1 to Samples_RenameSaga_MyTimeoutSagaVersion2");

#region renameTables

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;Integrated Security=True;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlPersistenceRenameSaga;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

using var connection = new SqlConnection(connectionString);

await connection.OpenAsync();

using var renameReplySaga = connection.CreateCommand();

renameReplySaga.CommandText = "exec sp_rename 'Samples_RenameSaga_MyReplySagaVersion1', 'Samples_RenameSaga_MyReplySagaVersion2'";
await renameReplySaga.ExecuteNonQueryAsync();

using var renameTimeoutSaga = connection.CreateCommand();

renameTimeoutSaga.CommandText = "exec sp_rename 'Samples_RenameSaga_MyTimeoutSagaVersion1', 'Samples_RenameSaga_MyTimeoutSagaVersion2'";
await renameTimeoutSaga.ExecuteNonQueryAsync();

#endregion

var endpointConfiguration = new EndpointConfiguration("Samples.RenameSaga");
SharedConfiguration.Apply(endpointConfiguration);

#region registerMutator

endpointConfiguration.RegisterMessageMutator(new ReplyMutator());

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Waiting to receive timeout and reply. Should happen within 10 seconds");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
