namespace EventBus.Messages.Models;
public class InstrumentStatus
{
    public string PackageID { get; set; } = string.Empty;
    public List<DeviceStatus> DeviceStatuses { get; set; } = new List<DeviceStatus>();
}
