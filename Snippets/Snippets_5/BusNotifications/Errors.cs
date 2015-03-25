using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reactive.Linq;
using NServiceBus;
using NServiceBus.Faults;
#region SubscribeToErrorsNotifications
public class SubscribeToErrorsNotifications : IWantToRunWhenBusStartsAndStops, IDisposable
{
    BusNotifications busNotifications;
	List<IDisposable> unsubscribeStreams = new List<IDisposable>();
	bool disposed;

	public SubscribeToErrorsNotifications(BusNotifications busNotifications)
    {
        this.busNotifications = busNotifications;
    }

	public void Start()
    {
		CheckIfDisposed();

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
		CheckIfDisposed();

        foreach (IDisposable unsubscribeStream in unsubscribeStreams)
        {
            unsubscribeStream.Dispose();
        }
		unsubscribeStreams.Clear();
    }

    void SendEmailOnFailure(FailedMessage failedMessage)
    {
        using (SmtpClient c = new SmtpClient())
        {

            using (MailMessage mailMessage = new MailMessage("from@mail.com",
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

	void CheckIfDisposed()
	{
		if (disposed)
			throw new Exception("Object has been disposed.");
    }

	public void Dispose()
	{
		Stop();
		disposed = true;
    }
}

#endregion
