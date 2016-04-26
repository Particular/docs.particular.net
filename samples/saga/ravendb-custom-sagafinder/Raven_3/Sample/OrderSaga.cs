﻿using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;
using System;

#region TheSagaRavenDB

public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<PaymentTransactionCompleted>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        //NOP
    }

    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        Data.PaymentTransactionId = Guid.NewGuid().ToString();

        logger.InfoFormat("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId);
        Bus.SendLocal(new IssuePaymentRequest
                      {
                          PaymentTransactionId = Data.PaymentTransactionId
                      });
    }

    public void Handle(PaymentTransactionCompleted message)
    {
        logger.InfoFormat("Transaction with Id {0} completed for order id {1}", Data.PaymentTransactionId, Data.OrderId);
        Bus.SendLocal(new CompleteOrder
                      {
                          OrderId = Data.OrderId
                      });
    }

    public void Handle(CompleteOrder message)
    {
        logger.InfoFormat("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId);
        MarkAsComplete();
    }
}

#endregion
