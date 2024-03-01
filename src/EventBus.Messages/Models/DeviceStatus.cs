using System.Text.Json.Serialization;
using EventBus.Messages.Enums;

namespace EventBus.Messages.Models;

public class DeviceStatus
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ModuleCategoryID ModuleCategoryID { get; set; }
		public int IndexWithinRole { get; set; }
		public RapidControlStatus RapidControlStatus { get; set; }
	}
