using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Extensibility;
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

        Assert.That(recoverabilityAction, Is.InstanceOf<Discard>(), "Message should be discarded");
    }

    #endregion

    [Test]
    public void ShouldFailImmediatelyAndSendToCustomErrorQueue()
    {
        HashSet<Type> unrecoverableExceptions = [typeof(DivideByZeroException)];

        var policy = CreatePolicy(unrecoverableExceptions: unrecoverableExceptions);
        var errorContext = CreateErrorContext(numberOfDeliveryAttempts: 1, exception: new DivideByZeroException());

        var recoverabilityAction = policy(errorContext);

        Assert.Multiple(() =>
        {
            Assert.That(recoverabilityAction, Is.InstanceOf<MoveToError>(), "Should ignore policies and move directly to custom error queue.");
            Assert.That(((MoveToError)recoverabilityAction).ErrorQueue, Is.EqualTo(CustomErrorQueue));
        });
    }

    [Test]
    public void ShouldBeMovedToDefaultErrorQueue()
    {
        var policy = CreatePolicy();
        var errorContext = CreateErrorContext(numberOfDeliveryAttempts: 1, exception: new Exception());

        var recoverabilityAction = policy(errorContext);

        Assert.Multiple(() =>
        {
            Assert.That(recoverabilityAction, Is.InstanceOf<MoveToError>(), "Should be moved to error queue.");
            Assert.That(((MoveToError)recoverabilityAction).ErrorQueue, Is.EqualTo(DefaultErrorQueue), "Should be moved to default error queue.");
        });
    }

    private const string DefaultErrorQueue = "errorQueue";
    private const string CustomErrorQueue = "customErrorQueue";

    #region create-error-context

    static ErrorContext CreateErrorContext(int numberOfDeliveryAttempts = 1, int? retryNumber = null, Dictionary<string, string> headers = null, Exception exception = null) =>
        new ErrorContext(
            exception ?? new Exception(),
            retryNumber.HasValue
                ? new Dictionary<string, string> { { Headers.DelayedRetries, retryNumber.ToString() } }
                : headers ?? [],
            "message-id",
            new ReadOnlyMemory<byte>([]),
            new TransportTransaction(),
            numberOfDeliveryAttempts,
            "receive-address",
            new ContextBag());

    #endregion

    #region create-policy

    static Func<ErrorContext, RecoverabilityAction> CreatePolicy(int maxImmediateRetries = 2, int maxDelayedRetries = 2, TimeSpan? delayedRetryDelay = null, HashSet<Type> unrecoverableExceptions = null)
    {
        var failedConfig = new FailedConfig("errorQueue", unrecoverableExceptions ?? []);
        var config = new RecoverabilityConfig(new ImmediateConfig(maxImmediateRetries), new DelayedConfig(maxDelayedRetries, delayedRetryDelay.GetValueOrDefault(TimeSpan.FromSeconds(2))), failedConfig);
        return context => CustomizedRecoverabilityPolicy.MyCustomRetryPolicy(config, context);
    }

    #endregion
}
