using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ
{
	public class RabbitMqOptions
	{
		public string DefaultTopic { get; set; } = string.Empty;

		public string Exchange { get; set; } = string.Empty;

		public string HostName { get; set; } = string.Empty;

		public string VHost { get; set; } = string.Empty;

		public string Username { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public ushort PrefetchCount { get; set; } = 1;

		public int TimeoutSeconds { get; set; } = 300;
	}
}
