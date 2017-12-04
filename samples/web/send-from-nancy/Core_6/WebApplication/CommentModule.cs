using Nancy;
using Nancy.TinyIoc;
using NServiceBus;
using System.Threading.Tasks;

namespace Web
{
    public class CommentModule : NancyModule
    {
        private readonly IDbContext dbContext;
        private readonly IMessageSession messageSession;

        public CommentModule(IDbContext dbContext, IMessageSession messageSession) : base("/comment")
        {
            this.dbContext = dbContext;
            this.messageSession = messageSession;
            
            this.Get["/", true] = async (r, c) => 
            {
                return await Task.Run(() => this.dbContext.GetData());
            };
        }
    }

    public class RootModule : NancyModule
    {
        public RootModule() : base()
        {
            this.Get["/"] = r => this.Response.AsRedirect("/comment");
        }
    }

    public class Bootstraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            #region EndpointConfiguration

            var endpointConfiguration = new EndpointConfiguration("Samples.Nancy.Sender");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.SendOnly();

            #endregion

            #region Routing

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.Nancy.Endpoint");

            #endregion

            #region EndpointStart

            var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            #endregion

            container.Register<IMessageSession>(endpointInstance);
        }
    }

    public interface IDbContext
    {
        string GetData();
    }

    public class DbContext : IDbContext
    {
        private int count = 0;

        public string GetData()
        {
            return (++count).ToString();
        }
    }
}