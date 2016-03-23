﻿// ReSharper disable UnusedParameter.Local
namespace Snippets6.Notifications
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Logging;

    #region SubscribeToErrorsNotifications

    public class SubscribeToNotifications :
        IWantToRunWhenBusStartsAndStops
    {
        Notifications notifications;
        static ILog log = LogManager.GetLogger<SubscribeToNotifications>();

        public SubscribeToNotifications(Notifications notifications)
        {
            this.notifications = notifications;
        }

        public Task Start(IMessageSession session)
        {
            ErrorsNotifications errors = notifications.Errors;
            errors.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => LogToConsole(retry);
            errors.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => LogToConsole(retry);
            errors.MessageSentToErrorQueue += (sender, retry) => LogToConsole(retry);
            return Task.FromResult(0);
        }

        void LogToConsole(FailedMessage failedMessage)
        {
            log.Info("Mesage sent to error queue");
        }

        void LogToConsole(SecondLevelRetry secondLevelRetry)
        {
            log.Info("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
        }

        void LogToConsole(FirstLevelRetry firstLevelRetry)
        {
            log.Info("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
        }

        public Task Stop(IMessageSession session)
        {
            return Task.FromResult(0);
        }
    }

    #endregion
}

