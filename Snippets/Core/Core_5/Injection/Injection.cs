namespace Core5.Injection
{
    using System.Net.Mail;
    using NServiceBus;

    class Injection
    {
        Injection(BusConfiguration busConfiguration)
        {
            #region ConfigurePropertyInjectionForHandler

            busConfiguration.RegisterComponents(c =>
                c.ConfigureComponent<EmailHandler>(DependencyLifecycle.InstancePerUnitOfWork)
                    .ConfigureProperty(x => x.SmtpAddress, "10.0.1.233")
                    .ConfigureProperty(x => x.SmtpPort, 25)
                );

            #endregion

            #region ConfigurePropertyInjectionForHandlerExplicit

            busConfiguration.InitializeHandlerProperty<EmailHandler>("SmtpAddress", "10.0.1.233");
            busConfiguration.InitializeHandlerProperty<EmailHandler>("SmtpPort", 25);

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
                var client = new SmtpClient(SmtpAddress, SmtpPort);
                // ...
            }
        }

        #endregion
    }

    public class EmailMessage
    {
    }
}