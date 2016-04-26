﻿namespace Core4.Injection
{
    using System.Net.Mail;
    using NServiceBus;

    class Injection
    {
        Injection(Configure configure)
        {
            #region ConfigurePropertyInjectionForHandler

            configure.DefaultBuilder();
            var configureComponents = configure.Configurer;
            configureComponents.ConfigureProperty<EmailHandler>(handler => handler.SmtpAddress, "10.0.1.233");
            configureComponents.ConfigureProperty<EmailHandler>(handler => handler.SmtpPort, 25);

            #endregion
        }

        #region PropertyInjectionWithHandler

        public class EmailHandler : IHandleMessages<EmailMessage>
        {
            public string SmtpAddress { get; set; }
            public int SmtpPort { get; set; }

            public void Handle(EmailMessage message)
            {
                SmtpClient client = new SmtpClient(SmtpAddress, SmtpPort);
                // ...
            }
        }

        #endregion
    }

    public class EmailMessage
    {
    }
}