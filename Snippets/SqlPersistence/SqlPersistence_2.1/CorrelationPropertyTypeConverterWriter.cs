using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus.Persistence.Sql.ScriptBuilder;
using NUnit.Framework;

[TestFixture]
public class CorrelationPropertyTypeConverterWriter
{
    [Test]
    public void Write()
    {
        var testDirectory = TestContext.CurrentContext.TestDirectory;
        var path = Path.Combine(testDirectory, "../../../../../nservicebus/sql-persistence/correlationpropertytypes.include.md");
        path = Path.GetFullPath(path);
        File.Delete(path);
        using (var writer = File.CreateText(path))
        {
            writer.WriteLine(@"

#### Microsoft SQL Server

| CorrelationPropertyType | Sql Type |
|--|--|");
            foreach (var type in GetValues())
            {
                var columnType = MsSqlServerCorrelationPropertyTypeConverter.GetColumnType(type);
                writer.WriteLine($"| `{type}` | `{columnType}` |");
            }

            writer.WriteLine(@"

#### MySQL

| CorrelationPropertyType | Sql Type |
|--|--|");
            foreach (var type in GetValues().Where(x => x != CorrelationPropertyType.DateTimeOffset))
            {
                var columnType = MySqlCorrelationPropertyTypeConverter.GetColumnType(type);
                writer.WriteLine($"| `{type}` | `{columnType}` |");
            }

            writer.WriteLine(@"

#### Oracle

| CorrelationPropertyType | Sql Type |
|--|--|");
            foreach (var type in GetValues().Where(x => x != CorrelationPropertyType.DateTimeOffset))
            {
                var columnType = OracleCorrelationPropertyTypeConverter.GetColumnType(type);
                writer.WriteLine($"| `{type}` | `{columnType}` |");
            }
        }
    }

    static IEnumerable<CorrelationPropertyType> GetValues()
    {
        return Enum.GetValues(typeof(CorrelationPropertyType)).Cast<CorrelationPropertyType>();
    }
}