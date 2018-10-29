using NServiceBus;
using System.ComponentModel.DataAnnotations;

#region DataAnnotations-Validator
public class MyMessage : IMessage
{
    [Required]
    public string Content { get; set; }
}
#endregion