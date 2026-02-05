using MySqlConnector;
using NServiceBus.Persistence.Sql;

var connectionString = "server=localhost;user=root;database=dbname;port=3306;password=pass;AllowUserVariables=True;AutoEnlist=false";

var scriptsDirectory = Directory.EnumerateDirectories(".", "NServiceBus.Persistence.Sql", SearchOption.AllDirectories).First(d => !d.Contains("/obj"));

await ScriptRunner.Install(
    sqlDialect: new SqlDialect.MySql(),
    tablePrefix: "Samples_Aurora_Lambda_Sales_",
    connectionBuilder: () => new MySqlConnection(connectionString),
    scriptDirectory: $"{scriptsDirectory}/MySql",
    shouldInstallOutbox: false,
    shouldInstallSagas: true,
    shouldInstallSubscriptions: false,
    cancellationToken: CancellationToken.None);