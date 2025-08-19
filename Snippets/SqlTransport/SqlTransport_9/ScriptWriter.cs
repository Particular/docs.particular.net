using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus;
using NUnit.Framework;

public class ScriptWriter
{
    readonly string directory;

    public ScriptWriter()
    {
        directory = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../RuntimeScripts");
        directory = Path.GetFullPath(directory);
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }

        Directory.CreateDirectory(directory);
    }

    [Test]
    public void Write()
    {
        // These are not used in snippets
        var ignoredProperties = new[]
        {
            "AddMessageBodyStringColumn",
            "CheckIfNonClusteredRowVersionIndexIsPresent",
            "CheckIfTableHasRecoverableText",
            "SendTextWithRecoverable",
        };

        // These property names have changed over time. We use aliases to ensure the snippets are still valid.
        // { "{New Name}", "{Old Name}" }
        // Consider using partial pattern in docs when snippet updates are beyond simple name changes.
        Dictionary<string, string> propertyAliases = new()
        {
            { "SendTextWithoutRecoverable", "SendText" },
        };

        var type = typeof(SqlServerTransport).Assembly.GetType("NServiceBus.Transport.SqlServer.SqlServerConstants",
            throwOnError: true);

        var sqlServerConstants = Activator.CreateInstance(type);
        foreach (var propertyInfo in type.GetProperties())
        {
            if (ignoredProperties.Contains(propertyInfo.Name))
            {
                continue;
            }

            Write(propertyAliases.TryGetValue(propertyInfo.Name, out var propertyName)
                    ? propertyName
                    : propertyInfo.Name,
                propertyInfo.GetValue(sqlServerConstants) as string);
        }
    }

    private void Write(string suffix, string script)
    {
        var path = Path.Combine(directory, $"{suffix}.sql");
        File.Delete(path);
        using var writer = File.CreateText(path);

        writer.WriteLine($@"startcode	{suffix}Sql".Replace("\t", " "));
        writer.WriteLine(script);
        writer.WriteLine("endcode");
    }
}