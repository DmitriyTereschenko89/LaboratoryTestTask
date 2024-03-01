using EasyNetQ;
using EasyNetQ.Topology;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using RabbitMQ;

namespace HostedServiceXmlParser.Messaging;

	public class MessageService : IMessageService
	{
		private readonly IBus _bus;
		private readonly ILogger<MessageService> _logger;

		public MessageService(IBus bus, ILogger<MessageService> logger)
		{
			_bus = bus ?? throw new ArgumentNullException(nameof(bus));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task PublishMessage(XmlReaderEvent msg)
		{
			// Declare exchange if necessary
			var exchange = _bus.Advanced.ExchangeDeclare(EventBusConstants.XmlParserExchange, ExchangeType.Topic);

			// Declare queue if necessary
			var queue = _bus.Advanced.QueueDeclare(EventBusConstants.XmlParserQueue);

			// Bind the queue to the exchange
			_bus.Advanced.Bind(exchange, queue, PublishExtensions.DefaultRoutigKey); // Binding key "#": match all routing keys

			// Publish the message
			await _bus.Advanced.PublishAsync(exchange, PublishExtensions.DefaultRoutigKey, false, new Message<XmlReaderEvent>(msg));
			_logger.LogInformation($"{nameof(XmlReaderEvent)} published successfully. CorrelationId = {msg.CorrelationId}");
		}
	}
