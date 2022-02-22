﻿#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace Creation
{
    using NServiceBus;
    using System.Threading.Tasks;

    #region Creation-SagaStart
    class ShipOrderWorkflow :
        Saga<ShipOrderWorkflow.ShipOrderData>,
        IAmStartedByMessages<ShipOrder>
    {
        public async Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
        }

        internal class ShipOrderData : ContainSagaData
        {
            public string OrderId { get; set; }
        }
        // ...
        #endregion

        #region Creation-ConfigureHowToFindSaga
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.MapSaga(saga => saga.OrderId)
                .ToMessage<ShipOrder>(message => message.OrderId);
        }
        #endregion
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
