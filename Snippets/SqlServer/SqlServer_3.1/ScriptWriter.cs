using System.IO;
using System.Reflection;
using NServiceBus.Transport.SQLServer;
using NUnit.Framework;

public class ScriptWriter
{

    string directory;

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
        var type = typeof(SqlConstants);
        foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic| BindingFlags.Public))
        {
            if (field.Name == "ExpiresIndexName")
            {
                continue;
            }
            Write(field.Name, (string) field.GetValue(null));
        }
    }

    void Write(string suffix, string script)
    {
        var path = Path.Combine(directory, $"{suffix}.sql");
        File.Delete(path);
        using (var writer = File.CreateText(path))
        {
            writer.WriteLine($@"startcode	{suffix}Sql".Replace("\t", " "));
            writer.WriteLine(script);
            writer.WriteLine("endcode");
        }
    }
}