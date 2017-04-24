using NServiceBus;

static class EntityFrameworkUnitOfWorkContextExtensions
{
    public static ReceiverDataContext DataContext(this IMessageHandlerContext context)
    {
        var uow = context.Extensions.Get<EntityFrameworkUnitOfWork>();
        return uow.GetDataContext(context.SynchronizedStorageSession);
    }
}