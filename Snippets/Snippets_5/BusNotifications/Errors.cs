namespace Snippets_5.BusNotifications
{
    #region SubscribeToErrorsNotifications

    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Reactive.Linq;
    using NServiceBus;
    using NServiceBus.Faults;

    public class SubscribeToErrorsNotifications : IWantToRunWhenBusStartsAndStops
    {
        BusNotifications busNotifications;

        public SubscribeToErrorsNotifications(BusNotifications busNotifications)
        {
            this.busNotifications = busNotifications;
        }

        public void Start()
        {
            unsubscribeStreams.Add(
                busNotifications.Errors.MessageSentToErrorQueue
                    // It is very important to handle streams on another thread
                    // otherwise the system performance can be impacted
                    .SubscribeOn(System.Reactive.Concurrency.Scheduler.Default) // Uses a pool-based scheduler
                    .Subscribe(SendEmailOnFailure)
                );


            // You can also subscribe when messages fail FLR and/or SLR
            // - busNotifications.Errors.MessageHasFailedAFirstLevelRetryAttempt
            // - busNotifications.Errors.MessageHasBeenSentToSecondLevelRetries
        }

        public void Stop()
        {
            foreach (var unsubscribeStream in unsubscribeStreams)
            {
                unsubscribeStream.Dispose();
            }
        }

        void SendEmailOnFailure(FailedMessage failedMessage)
        {
            using (var c = new SmtpClient())
            {

                using (var mailMessage = new MailMessage("from@mail.com", 
                    "to@mail.com", "Message sent to error queue", 
                    failedMessage.Exception.ToString()))
                {
                    try
                    {
                        c.Send(mailMessage);
                    }
                    catch (SmtpFailedRecipientsException)
                    {
                        // Failed to send an email to some of its recipients
                        // Probably you should log this as a warning!
                    }
                }
            }
        }

        List<IDisposable> unsubscribeStreams = new List<IDisposable>();
    }
    #endregion
}
