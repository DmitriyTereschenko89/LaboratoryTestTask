namespace EventBus.Messages.Models;

public class CombinedSamplerStatus : RapidControlStatus
	{
		public int Status { get; set; }
		public string Vial { get; set; }
		public int Volume { get; set; }
		public int MaximumInjectionVolume { get; set; }
		public string RackL { get; set; }
		public string RackR { get; set; }
		public int RackInf { get; set; }
		public bool Buzzer { get; set; }
	}
