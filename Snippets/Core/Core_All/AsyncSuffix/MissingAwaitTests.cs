using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

class MissingAwaitTests
{
#pragma warning disable 649
    string pathToAssembly;
#pragma warning restore 649

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
        MissingTaskUsageDetector.CheckForMissingTaskUsage(pathToAssembly);
    }

    #endregion


    #region ExplicitlyIgnoreTask

    public static void ExplicitlyIgnoreTask()
    {
        var writer = new StreamReader("stub");
        // Note the returned instance is ignored
        writer.ReadLineAsync().IgnoreTask();
    }

    #endregion

}