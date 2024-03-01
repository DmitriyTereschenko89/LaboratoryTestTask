using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace RabbitMQ
{
	public static class PublishExtensions
	{
		public const string DefaultRoutigKey = "#";

		public static string AddRabbitMQ(this IServiceCollection services, RabbitMqOptions config, Action<IServiceRegister> registerServices = null)
		{
			string rabbitMqConnectionString = $"host={config.HostName};virtualHost={config.VHost};username={config.Username};password={config.Password};prefetchcount={config.PrefetchCount};timeout={config.TimeoutSeconds}";

			if (registerServices == null)
			{
				services.RegisterEasyNetQ(rabbitMqConnectionString);
			}
			else
			{
				services.RegisterEasyNetQ(rabbitMqConnectionString, registerServices);
			}

			return rabbitMqConnectionString;
		}

		public static async Task SendWithHeaders<T>(this IAdvancedBus bus, T message, IDictionary<string, object> messageHeaders, IExchange exchange, string messageTypeName, CancellationToken cancellationToken = default(CancellationToken), string routingKey = "#")
		{
			byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
			if (messageHeaders == null)
			{
				messageHeaders = new Dictionary<string, object>();
			}

			MessageProperties messageProperties = new()
			{
				Headers = messageHeaders,
				Type = messageTypeName
			};
			await bus.PublishAsync(exchange, routingKey, mandatory: true, messageProperties, bytes, cancellationToken);
		}
	}
}
