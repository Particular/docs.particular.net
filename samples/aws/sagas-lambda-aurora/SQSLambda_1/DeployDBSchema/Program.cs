using MySql.Data.MySqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

await ScriptRunner.Install(
    sqlDialect: new SqlDialect.MySql(),
    tablePrefix: "Samples_Aurora_Lambda_Sales_",
    connectionBuilder: () => new MySqlConnection("server=localhost;user=root;database=dbname;port=3306;password=pass;AllowUserVariables=True;AutoEnlist=false"),
    scriptDirectory: @"../../../../sales/bin/Debug/net6.0/NServiceBus.Persistence.Sql/MySql",
    shouldInstallOutbox: false,
    shouldInstallSagas: true,
    shouldInstallSubscriptions: false,
    cancellationToken: CancellationToken.None);