using System.ComponentModel.DataAnnotations;
using NServiceBus;

public class CreateProductCommand : ICommand
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
        return string.Format(
            "CreateProductCommand: ProductId={0}, ProductName={1}, ListPrice={2} Image (length)={3}",
            ProductId, ProductName, ListPrice, Image == null ? 0 : Image.Length);
    }
}

