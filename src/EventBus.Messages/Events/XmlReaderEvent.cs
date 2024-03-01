namespace EventBus.Messages.Events;


public class XmlReaderEvent : IntegrationBaseEvent
{
	public string DeviceStatusesJson { get; set; }
}
