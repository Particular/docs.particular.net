﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9.BuyersRemorseTimeoutRequest;

class BuyersRemorsePolicy(ILogger<BuyersRemorsePolicy> logger) : Saga<BuyersRemorseData>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseData> mapper)
    {

    }

    #region BuyersRemorseTimeoutRequest

    public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", message.OrderId);
        Data.OrderId = message.OrderId;

        logger.LogInformation("Starting cool down period for order #{OrderId}.", Data.OrderId);
        await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());
    }

    #endregion
}

internal class BuyersRemorseIsOver
{

}

public class BuyersRemorseData : ContainSagaData
{
    public string OrderId { get; set; }
}

internal class PlaceOrder
{
    public string OrderId { get; set; }
}