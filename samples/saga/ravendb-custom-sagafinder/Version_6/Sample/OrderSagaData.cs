﻿using NServiceBus;
using Raven.Client.UniqueConstraints;

#region OrderSagaDataRavenDB

public class OrderSagaData : ContainSagaData
{
    [UniqueConstraint(CaseInsensitive = true)]
    public string OrderId { get; set; }

    [UniqueConstraint(CaseInsensitive = true)]
    public string PaymentTransactionId { get; set; }
}

#endregion