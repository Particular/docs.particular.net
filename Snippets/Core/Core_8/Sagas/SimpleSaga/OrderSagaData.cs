﻿namespace Core7.Sagas.SimpleSaga
{
    using NServiceBus;

    #region simple-saga-data
    public class OrderSagaData :
        ContainSagaData
    {
        public string OrderId { get; set; }
    }
    #endregion
}
