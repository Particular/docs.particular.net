using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

[SetUpFixture]
public static class Global
{
    [OneTimeSetUp]
    public static void SetupInvariantCulture()
    {
        var current = Thread.CurrentThread;
        current.CurrentCulture = CultureInfo.InvariantCulture;
        current.CurrentUICulture = CultureInfo.InvariantCulture;
    }
}