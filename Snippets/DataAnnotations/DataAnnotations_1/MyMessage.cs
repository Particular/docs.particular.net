using System.ComponentModel.DataAnnotations;
using NServiceBus;

#region DataAnnotations_message
public class MyMessage : IMessage
{
    [Required]
    public string Content { get; set; }
}
#endregion