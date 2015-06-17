using System;
using NServiceBus.Saga;

namespace Snippets6.Sagas.SimpleSaga
{

    #region simple-saga-data
    public class OrderSagaData : ContainSagaData
    {   
        public string OrderId { get; set; }
    }
    #endregion
}
