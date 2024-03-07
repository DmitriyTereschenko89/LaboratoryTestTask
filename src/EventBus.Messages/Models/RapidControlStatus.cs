using System.Text.Json.Serialization;
using System.Xml.Serialization;
using EventBus.Messages.Enums;

namespace EventBus.Messages.Models;

[XmlInclude(typeof(CombinedSamplerStatus))]
[XmlInclude(typeof(CombinedPumpStatus))]
[XmlInclude(typeof(CombinedOvenStatus))]
public abstract class RapidControlStatus
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [XmlElement("ModuleState")]
    public ModuleState ModuleState { get; set; }
    [XmlElement("IsBusy")]
    public bool IsBusy { get; set; }
    [XmlElement("IsReady")]
    public bool IsReady { get; set; }
    [XmlElement("IsError")]
    public bool IsError { get; set; }
    [XmlElement("KeyLock")]
    public bool KeyLock { get; set; }
}
