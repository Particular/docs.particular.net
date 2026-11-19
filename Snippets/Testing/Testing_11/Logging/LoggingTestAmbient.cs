using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Testing;
using NUnit.Framework;

#pragma warning disable CS0618 // Type or member is obsolete

[Explicit]
#region LoggerTestingAmbient
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

        await handler.Handle(new MyRequest(), new TestableMessageHandlerContext());

        Assert.That(logStatements.ToString(), Does.Contain("Some log message"));
    }
}

#endregion

#pragma warning restore CS0618 // Type or member is obsolete