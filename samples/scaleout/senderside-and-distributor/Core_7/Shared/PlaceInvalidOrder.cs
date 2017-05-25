using System;

public class PlaceInvalidOrder :
    IMessage
{
    public Guid OrderId { get; set; }
}