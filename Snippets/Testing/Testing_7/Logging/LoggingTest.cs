using System;
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

        await handler.Handle(new MyRequest(), new TestableMessageHandlerContext())
            .ConfigureAwait(false);

        StringAssert.Contains("Some log message", LoggingSetupFixture.LogStatements);
    }

    #endregion
}

#region LoggerTestingAmbient [7.2,8.0)
[TestFixture]
public class LoggingTestsAmbient
{


    StringBuilder logStatements;
    IDisposable scope;
    
    [SetUp]
    public void SetUp()
    {
        logStatements = new StringBuilder();
        
        scope = LogManager.Use<TestingLoggerFactory>()
            .BeginScope(new StringWriter(logStatements));
    }
    
    [TearDown]
    public void Teardown()
    {
        scope.Dispose();
    }
    
    [Test]
    public async Task ShouldLogCorrectly()
    {
        var handler = new MyHandlerWithLogging();

        await handler.Handle(new MyRequest(), new TestableMessageHandlerContext())
            .ConfigureAwait(false);

        StringAssert.Contains("Some log message", logStatements.ToString());
    }
}
#endregion