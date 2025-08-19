using System.IO;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Testing;
using NUnit.Framework;

#region LoggerTestingSetup
[SetUpFixture]
public class LoggingSetupFixture
{
    static StringBuilder logStatements = new StringBuilder();

    [OneTimeSetUp]
    public void SetUp()
    {
        LogManager.Use<TestingLoggerFactory>()
            .WriteTo(new StringWriter(logStatements));
    }

    public static string LogStatements => logStatements.ToString();

    public static void Clear()
    {
        logStatements.Clear();
    }
}
#endregion

[Explicit]
[TestFixture]
public class LoggingTests
{
    #region LoggerTesting

    [SetUp]
    public void SetUp()
    {
        LoggingSetupFixture.Clear();
    }

    [Test]
    public async Task ShouldLogCorrectly()
    {
        var handler = new MyHandlerWithLogging();

        await handler.Handle(new MyRequest(), new TestableMessageHandlerContext());

        Assert.That(LoggingSetupFixture.LogStatements, Does.Contain("Some log message"));
    }

    #endregion
}