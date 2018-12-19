using System;

public class Shipment
{
    public virtual Guid Id { get; set; }
    public virtual Order Order { get; set; }
    public virtual string Location { get; set; }
}