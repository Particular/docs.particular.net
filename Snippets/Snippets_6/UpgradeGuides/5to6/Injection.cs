namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Injection
    {
        Injection(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-ExplicitProperties 

            endpointConfiguration.RegisterComponents(c =>
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
                return null;
            }
        }
        public class EmailMessage
        {
        }
    }

  
}