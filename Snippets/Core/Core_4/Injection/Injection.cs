namespace Core4.Injection
{
    using System.Net.Mail;
    using NServiceBus;

    class Injection
    {
        Injection(Configure configure)
        {
            #region ConfigurePropertyInjectionForHandler

            configure.DefaultBuilder();
            var components = configure.Configurer;
            components.ConfigureProperty<EmailHandler>(handler => handler.SmtpAddress, "10.0.1.233");
            components.ConfigureProperty<EmailHandler>(handler => handler.SmtpPort, 25);

            #endregion
        }

        #region PropertyInjectionWithHandler

        public class EmailHandler :
            IHandleMessages<EmailMessage>
        {
            public string SmtpAddress { get; set; }
            public int SmtpPort { get; set; }

            public void Handle(EmailMessage message)
            {
                using (var client = new SmtpClient(SmtpAddress, SmtpPort))
                {
                    // use client
                }
            }
        }

        #endregion
    }

    public class EmailMessage
    {
    }
}