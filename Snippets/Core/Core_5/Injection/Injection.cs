namespace Core5.Injection
{
    using System.Net.Mail;
    using NServiceBus;

    class Injection
    {
        Injection(BusConfiguration busConfiguration)
        {
            #region ConfigurePropertyInjectionForHandler

            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    var component = components.ConfigureComponent<EmailHandler>(DependencyLifecycle.InstancePerUnitOfWork);
                    component.ConfigureProperty(handler => handler.SmtpAddress, "10.0.1.233");
                    component.ConfigureProperty(handler => handler.SmtpPort, 25);
                });

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