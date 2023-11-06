﻿using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class StageInventoryHandler : IHandleMessages<OrderReceived>
{
  ILog log = LogManager.GetLogger<StageInventoryHandler>();

  public async Task Handle(OrderReceived message, IMessageHandlerContext context)
  {
    log.Info($"Staging inventory for order {message.OrderId}.");

    await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 5)));

    await context.Publish(new InventoryStaged { OrderId = message.OrderId });
  }
}
