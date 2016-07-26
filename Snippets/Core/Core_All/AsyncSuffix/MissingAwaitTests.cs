using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

class MissingAwaitTests
{
    #region TaskCasesNotDetectedByTheCompiler

    public static Task MissingTaskUsage1()
    {
        var writer = new StreamReader("stub");

        // Note the returned instance is not used
        writer.ReadLineAsync();

        return writer.ReadLineAsync();
    }

    public static void MissingTaskUsage2()
    {
        var writer = new StreamReader("stub");

        // Note the returned instance is not used
        writer.ReadLineAsync();
    }

    #endregion

    [Explicit]
    #region MissingTaskUsageDetectorUsage

    [Test]
    public void TestForMissingTaskUsage()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var assemblyPath = executingAssembly.CodeBase.Substring(8);
        MissingTaskUsageDetector.CheckForMissingTaskUsage(assemblyPath);
    }

    #endregion


    #region ExplictlyIgnoreTask

    public static void ExplictlyIgnoreTask()
    {
        var writer = new StreamReader("stub");
        // Note the returned instance is ignored
        writer.ReadLineAsync().IgnoreTask();
    }

    #endregion

}