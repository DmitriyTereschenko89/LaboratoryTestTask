using System.Text.Json.Serialization;
using EventBus.Messages.Enums;

namespace EventBus.Messages.Models;

public abstract class RapidControlStatus
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ModuleState ModuleState { get; set; }
		public bool IsBusy { get; set; }
		public bool IsReady { get; set; }
		public bool IsError { get; set; }
		public bool KeyLock { get; set; }
	}
