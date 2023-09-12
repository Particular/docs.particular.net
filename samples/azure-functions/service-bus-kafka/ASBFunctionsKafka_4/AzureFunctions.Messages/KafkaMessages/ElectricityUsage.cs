using Newtonsoft.Json;

namespace AzureFunctions.Messages.KafkaMessages;

public class ElectricityUsage
{
    public int CustomerId { get; set; }
    public int UnitId { get; set; }
    public int CurrentUsage { get; set; }

    public static string Serialize(ElectricityUsage electricityUsage)
    {
        return JsonConvert.SerializeObject(electricityUsage);
    }

    public static ElectricityUsage Deserialize(string value)
    {
        return JsonConvert.DeserializeObject<ElectricityUsage>(value);
    }
}