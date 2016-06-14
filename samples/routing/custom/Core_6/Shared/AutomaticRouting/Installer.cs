using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus.Installation;
using NServiceBus.Settings;

class Installer : INeedToInstallSomething
{
    internal const string CreateTableText = @"IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Data]') AND type in (N'U'))
                  BEGIN
                    EXEC sp_getapplock @Resource = 'dbo_Data_lock', @LockMode = 'Exclusive'

                    IF NOT  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Data]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE [dbo].[Data](
	                        [Publisher] [varchar](100) NOT NULL,
	                        [Value] [nvarchar](max) NULL
                        ) ON [PRIMARY];
                    END

                    EXEC sp_releaseapplock @Resource = 'dbo_Data_lock'
                  END";

    ReadOnlySettings settings;

    public Installer(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public async Task Install(string identity)
    {
        string connectionString;
        if (!settings.TryGet("NServiceBus.DataBackplane.ConnectionString", out connectionString))
        {
            return;
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand(CreateTableText, connection, transaction)
                {
                    CommandType = CommandType.Text
                })
                {
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                transaction.Commit();
            }
        }
    }
}