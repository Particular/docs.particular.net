﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Testing;
using NUnit.Framework;

#region LoggerTestingSetup
[SetUpFixture]
public class LoggingSetupFixture
{
    static readonly StringBuilder logStatements = new StringBuilder();

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