namespace Snippets6.UpgradeGuides._5to6
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using NServiceBus;

    public class Injection
    {
        public void ExplicitProperties()
        {
            EndpointConfiguration busConfiguration = new EndpointConfiguration();

            #region ExplicitProperties 

            busConfiguration.RegisterComponents(c =>
                c.ConfigureComponent(builder => new MyHandler
                {
                    MyIntProperty = 25,
                    MyStringProperty = "Some string"
                }, DependencyLifecycle.InstancePerUnitOfWork));

            #endregion
        }

   
        public class MyHandler : IHandleMessages<EmailMessage>
        {
            public string MyStringProperty { get; set; }
            public int MyIntProperty { get; set; }

            public Task Handle(EmailMessage message, IMessageHandlerContext context)
            {
                SmtpClient client = new SmtpClient(MyStringProperty, MyIntProperty);
                // ...

                return Task.FromResult(0);
            }
        }
        public class EmailMessage
        {
        }
    }

  
}