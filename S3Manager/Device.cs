using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Devices")]
public class Device
{
    [DynamoDBHashKey] // Partition Key
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public int BatteryLevel { get; set; }
    public DateTime LastReport { get; set; }
}
