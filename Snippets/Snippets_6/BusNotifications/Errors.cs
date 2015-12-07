namespace Snippets6.BusNotifications
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Faults;

    #region SubscribeToErrorsNotifications

    public class SubscribeToErrorsNotifications : IWantToRunWhenBusStartsAndStops
    {
        BusNotifications busNotifications;

        public SubscribeToErrorsNotifications(BusNotifications busNotifications)
        {
            this.busNotifications = busNotifications;
        }

        public Task Start(IBusContext context)
        {
            busNotifications.Errors.MessageSentToErrorQueue += Errors_MessageSentToErrorQueue;
            return Task.FromResult(0);

            // You can also subscribe when messages fail FLR and/or SLR
            // - busNotifications.Errors.MessageHasFailedAFirstLevelRetryAttempt
            // - busNotifications.Errors.MessageHasBeenSentToSecondLevelRetries
        }

        private void Errors_MessageSentToErrorQueue(object sender, FailedMessage e)
        {
            SendEmailOnFailure(e);
        }

        public Task Stop(IBusContext context)
        {
            return Task.FromResult(0);
        }

        void SendEmailOnFailure(FailedMessage failedMessage)
        {
            string body = failedMessage.Exception.ToString();
            using (SmtpClient c = new SmtpClient())
            using (MailMessage mailMessage = new MailMessage("from@mail.com",
                "to@mail.com", "Message sent to error queue", body))
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

    #endregion
}

