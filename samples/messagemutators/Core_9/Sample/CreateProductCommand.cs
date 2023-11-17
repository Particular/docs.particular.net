using System.ComponentModel.DataAnnotations;
using NServiceBus;

public class CreateProductCommand :
    ICommand
{
    [Required]
    public string ProductId { get; set; }

    [StringLength(20, ErrorMessage = "The Product Name value cannot exceed 20 characters. ")]
    public string ProductName { get; set; }

    [Range(1, 5)]
    public decimal ListPrice { get; set; }

    public byte[] Image { get; set; }

    public override string ToString()
    {
        return $"CreateProductCommand: ProductId={ProductId}, ProductName={ProductName}, ListPrice={ListPrice} Image (length)={Image?.Length ?? 0}";
    }
}