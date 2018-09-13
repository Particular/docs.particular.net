using System.IO;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class LoggingTests
{
    #region LoggerTesting
    [Test]
    public async Task ShouldLogCorrectly()
    {
        var logStatements = new StringBuilder();

        LogManager.Use<TestingLoggerFactory>()
            .WriteTo(new StringWriter(logStatements));

        var handler = new MyHandlerWithLogging();

        await handler.Handle(new MyRequest(), new TestableMessageHandlerContext())
            .ConfigureAwait(false);

        StringAssert.Contains("Some log message", logStatements.ToString());
    }
    #endregion
}