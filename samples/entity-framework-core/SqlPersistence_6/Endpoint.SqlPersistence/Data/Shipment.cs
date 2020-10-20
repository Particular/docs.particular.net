using System;
using System.ComponentModel.DataAnnotations;

public class Shipment
{
    public virtual Guid Id { get; set; }

    [Required]
    public virtual Order Order { get; set; }
    public virtual string Location { get; set; }
}