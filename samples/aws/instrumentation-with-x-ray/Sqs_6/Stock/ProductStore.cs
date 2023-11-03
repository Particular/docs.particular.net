using Domain;
using Events;

namespace Inventory;

public static class ProductStore
{
    public static List<Product> Products { get; } = new()
    {
        new() { ProductId = "DD3FC711-4693-44C5-B629-779F393C9C3A", ProductName = "Harry Potter and the philosopher's stone"},
        new() { ProductId = "6F433FC8-85D8-4AE0-99E9-21CEBB1EBC58", ProductName = "Harry Potter and the chamber of secrets"},
        new() { ProductId = "C2530BC3-AF29-4CB9-B081-4B08F483E74C", ProductName = "Harry Potter and the prisoner of Azkaban"},
        new() { ProductId = "2C57F727-8156-4E30-A849-4DB4D0A3024A", ProductName = "Harry Potter and the goblet of fire"},
        new() { ProductId = "AD3C7D1A-A094-4FDB-A92A-7DE66874E7F3", ProductName = "Harry Potter and the order of the phoenix"},
        new() { ProductId = "4648784F-FAAC-41F4-A637-86F32476696C", ProductName = "Harry Potter and the half-blood prince"},
        new() { ProductId = "0FB8CD78-DADF-41FE-B9FF-AE5793BFFE4B", ProductName = "Harry Potter and the deathly hallows"}
    };
}