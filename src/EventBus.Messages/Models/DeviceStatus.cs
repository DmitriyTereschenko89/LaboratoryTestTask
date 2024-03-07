using System.Text.Json.Serialization;
using System.Xml.Serialization;
using EventBus.Messages.Enums;

namespace EventBus.Messages.Models;

public class DeviceStatus
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [XmlElement("ModuleCategoryID")]
    public ModuleCategoryID ModuleCategoryID { get; set; }
    [XmlElement("IndexWithinRole")]
    public int IndexWithinRole { get; set; }
    [XmlElement("RapidControlStatus")]
    public RapidControlStatus RapidControlStatus { get; set; }
}
