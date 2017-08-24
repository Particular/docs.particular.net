using System;
using NServiceBus;

public class OrderSagaDataXml :
    ContainSagaData
{
    public virtual string OrderId { get; set; }
    public virtual int Version { get; set; }
    public virtual Location From { get; set; }
    public virtual Location To { get; set; }
    public virtual AmountInfo Total { get; set; }

    public class Location
    {
        public virtual Guid Id { get; set; }
        public virtual double Lat { get; set; }
        public virtual double Long { get; set; }
    }

    public class AmountInfo
    {
        public virtual string Currency { get; set; }
        public virtual decimal Amount { get; set; }
    }
}
