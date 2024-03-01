using DataProcessor.Processing;
using EasyNetQ;
using EasyNetQ.Topology;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using EventBus.Messages.Extentions;
using Polly;
using RabbitMQ;

namespace DataProcessor;

	public class XmlReaderHost : IHostedService
	{
		private readonly ILogger<XmlReaderHost> _logger;
		private readonly IBus _bus;
		private readonly Policy<bool> _reconnectionPolicy;
		private IDisposable _subscriptionResult;
		private readonly IProcessDataUpdate _processDataUpdate;

		/// <summary>
		/// GUID to distinguish this instance from service instances running in other docker containers
		/// </summary>
		internal static readonly string ServiceId = Guid.NewGuid().ToString();

		public XmlReaderHost(ILogger<XmlReaderHost> logger, IBus bus, IProcessDataUpdate processDataUpdate)
		{
			_processDataUpdate = processDataUpdate;
			_logger = logger;
			_bus = bus;
			_subscriptionResult = null;
			_reconnectionPolicy = Policy
							.Handle<Exception>()
										.OrResult(false)
										.WaitAndRetryForever(retryAttempt => (retryAttempt > 10) ? TimeSpan.FromMinutes(10) : TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
										(_, __) => _logger.LogInformation("Attempting to reconnect with rabbit"));
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_bus.Advanced.Connected += BusConnected;
			_bus.Advanced.Disconnected += BusDisconnected;
			_logger.LogInformation("Swoc log host starting MQ subscription");

			return Task.Run(new Action(() => _reconnectionPolicy.Execute(() => Subscribe())), cancellationToken);
		}

		public void BusConnected(object sender, ConnectedEventArgs e)
		{
			_logger.LogInformation("Swoc log bus connected {Hostname} {Port}", e.Hostname, e.Port);
		}

		/// <summary>
		/// Subscribes to the queue
		/// </summary>
		public bool Subscribe()
		{
			try
			{
				// Declare the exchange
				var exchange = _bus?.Advanced.ExchangeDeclare(EventBusConstants.XmlParserExchange, ExchangeType.Topic);

				// Declare the queue in quorum mode
				var queue = _bus?.Advanced.QueueDeclare(EventBusConstants.XmlParserQueue);

				// Bind the queue to the exchange with the routing key
				_bus?.Advanced.Bind(exchange, queue, PublishExtensions.DefaultRoutigKey);
				// Create the consumer
				_subscriptionResult = _bus?.Advanced.Consume<XmlReaderEvent>(queue, (msg, info, token) =>
				{
					var update = msg?.GetBody() as XmlReaderEvent ?? new XmlReaderEvent();

					return RetryHelperTask.
										RetryOnExceptionResultSimpleAsync(5,
																			TimeSpan.FromSeconds(0),
																			async () => await _processDataUpdate.ProcessAsync(update)
										);
				}, x => x.WithArgument("test", "test"));
			}
			catch (Exception ee)
			{
				_logger.LogError(ee, "Subscription error {Message}", ee.Message);
				_subscriptionResult?.Dispose();
				_subscriptionResult = null;
				return false;
			}

			return true;
		}

		public void BusDisconnected(object sender, DisconnectedEventArgs e)
		{
			_logger.LogInformation("XmlReaderHost bus disconnected {Reason} {Hostname} {Port}.", e.Reason, e.Hostname, e.Port);
			// re-subscribe
			Task.Run(new Action(() => _reconnectionPolicy.Execute(() => Subscribe())));
		}

		/// <inheritdoc />
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_bus.Advanced.Connected -= BusConnected;
			_bus.Advanced.Disconnected -= BusDisconnected;

			_logger.LogInformation($"{nameof(XmlReaderHost)} shutdown");
			_subscriptionResult?.Dispose();
			return Task.CompletedTask;
		}
	}
