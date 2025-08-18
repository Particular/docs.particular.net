using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate.Session;

public class OrderHandlerViaContext :
    IHandleMessages<OrderMessage>
{
    #region NHibernateAccessingDataViaContextHandler

    public Task Handle(OrderMessage message, IMessageHandlerContext context)
    {
        var nhibernateSession = context.SynchronizedStorageSession.Session();
        nhibernateSession.Save(new Order());
        return Task.CompletedTask;
    }

    #endregion
}