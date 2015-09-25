namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;
    using NServiceBus.Pipeline.Contexts;
    using NServiceBus.TransportDispatch;

    #region 5to6header-outgoing-behavior
    public class OutgoingBehavior : Behavior<OutgoingContext>
    {
        public override async Task Invoke(OutgoingContext context, Func<Task> next)
        {
            context.SetHeader("MyCustomHeader", "My custom value");
            await next();
        }
    }
    #endregion
}
