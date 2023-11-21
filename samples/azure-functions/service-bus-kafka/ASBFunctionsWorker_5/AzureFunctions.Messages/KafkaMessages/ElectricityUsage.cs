using System.Text.Json;

namespace AzureFunctions.Messages.KafkaMessages;

public class ElectricityUsage
{
    public int CustomerId { get; set; }
    public int UnitId { get; set; }
    public int CurrentUsage { get; set; }

    public static string Serialize(ElectricityUsage electricityUsage)
    {
        return JsonSerializer.Serialize(electricityUsage);
    }

    public static ElectricityUsage Deserialize(string value)
    {
        return JsonSerializer.Deserialize<ElectricityUsage>(value);
    }
}