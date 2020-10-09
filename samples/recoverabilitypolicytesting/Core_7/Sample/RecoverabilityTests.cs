using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Transport;
using NUnit.Framework;

[TestFixture]
public class RecoverabilityTests
{
    #region test-example

    [Test]
    public void ShouldDiscardBecauseOfCustomException()
    {
        var policy = CreatePolicy();
        var errorContext = CreateErrorContext(numberOfDeliveryAttempts: 1, exception: new MyBusinessTimedOutException());

        var recoverabilityAction = policy(errorContext);

        Assert.IsInstanceOf<Discard>(recoverabilityAction, "Message should be discarded");
    }

    #endregion

    [Test]
    public void ShouldFailImmediatelyAndSendToCustomErrorQueue()
    {
        var unrecoverableExceptions = new HashSet<Type> { typeof(DivideByZeroException) };

        var policy = CreatePolicy(unrecoverableExceptions: unrecoverableExceptions);
        var errorContext = CreateErrorContext(numberOfDeliveryAttempts: 1, exception: new DivideByZeroException());

        var recoverabilityAction = policy(errorContext);

        Assert.IsInstanceOf<MoveToError>(recoverabilityAction, "Should ignore policies and move directly to custom error queue.");
        Assert.AreEqual(CustomErrorQueue, ((MoveToError)recoverabilityAction).ErrorQueue);
    }

    [Test]
    public void ShouldBeMovedToDefaultErrorQueue()
    {
        var policy = CreatePolicy();
        var errorContext = CreateErrorContext(numberOfDeliveryAttempts: 1, exception: new Exception());

        var recoverabilityAction = policy(errorContext);

        Assert.IsInstanceOf<MoveToError>(recoverabilityAction, "Should be moved to error queue.");
        Assert.AreEqual(DefaultErrorQueue, ((MoveToError)recoverabilityAction).ErrorQueue, "Should be moved to default error queue.");
    }

    private const string DefaultErrorQueue = "errorQueue";
    private const string CustomErrorQueue = "customErrorQueue";

    #region create-error-context

    static ErrorContext CreateErrorContext(int numberOfDeliveryAttempts = 1, int? retryNumber = null, Dictionary<string, string> headers = null, Exception exception = null) =>
        new ErrorContext(
            exception ?? new Exception(),
            retryNumber.HasValue
                ? new Dictionary<string, string> { { Headers.DelayedRetries, retryNumber.ToString() } }
                : headers ?? new Dictionary<string, string>(),
            "message-id",
            new byte[0],
            new TransportTransaction(),
            numberOfDeliveryAttempts);

    #endregion

    #region create-policy

    static Func<ErrorContext, RecoverabilityAction> CreatePolicy(int maxImmediateRetries = 2, int maxDelayedRetries = 2, TimeSpan? delayedRetryDelay = null, HashSet<Type> unrecoverableExceptions = null)
    {
        var failedConfig = new FailedConfig("errorQueue", unrecoverableExceptions ?? new HashSet<Type>());
        var config = new RecoverabilityConfig(new ImmediateConfig(maxImmediateRetries), new DelayedConfig(maxDelayedRetries, delayedRetryDelay.GetValueOrDefault(TimeSpan.FromSeconds(2))), failedConfig);
        return context => CustomizedRecoverabilityPolicy.MyCustomRetryPolicy(config, context);
    }

    #endregion
}
