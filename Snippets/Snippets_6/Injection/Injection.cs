namespace Snippets5.Injection
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using NServiceBus;

    public class Injection
    {
        public void ConfigurePropertyInjectionForHandler()
        {
            EndpointConfiguration busConfiguration = new EndpointConfiguration();

            #region ConfigurePropertyInjectionForHandler 

            busConfiguration.RegisterComponents(c =>
                c.ConfigureComponent(builder => new EmailHandler
                {
                    SmtpPort = 25,
                    SmtpAddress = "10.0.1.233"
                }, DependencyLifecycle.InstancePerUnitOfWork));

            #endregion
        }

        #region PropertyInjectionWithHandler 

        public class EmailHandler : IHandleMessages<EmailMessage>
        {
            public string SmtpAddress { get; set; }
            public int SmtpPort { get; set; }

            public Task Handle(EmailMessage message, IMessageHandlerContext context)
            {
                SmtpClient client = new SmtpClient(SmtpAddress, SmtpPort);
                // ...

                return Task.FromResult(0);
            }
        }

        #endregion
    }

    public class EmailMessage
    {
    }
}