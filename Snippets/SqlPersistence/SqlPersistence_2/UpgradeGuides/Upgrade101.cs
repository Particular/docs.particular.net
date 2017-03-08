using System.Data.SqlClient;
using System.IO;

class Upgrade101
{
    void ExecuteConvertOutboxToNonclustered(string tablePrefix)
    {
        #region ExecuteConvertOutboxToNonclustered

        using (var connection = new SqlConnection("ConnectionString"))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = File.ReadAllText("PathToConvertOutboxToNonclustered.sql");
                var parameter = command.CreateParameter();
                parameter.ParameterName = "tablePrefix";
                parameter.Value = tablePrefix;
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }

}