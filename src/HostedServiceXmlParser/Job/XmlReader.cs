
using HostedServiceXmlParser.Workers;

namespace HostedServiceXmlParser.Job;

	public class XmlReader : IHostedService
	{
		private readonly ILogger<XmlReader> _logger;
		private readonly IWorker _worker;

		public XmlReader(ILogger<XmlReader> logger, IWorker worker)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_worker = worker ?? throw new ArgumentNullException(nameof(worker));
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{nameof(XmlReader)} is started at {DateTimeOffset.Now}");
			await _worker.DoWork(cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"{nameof(XmlReader)} is stoped at {DateTimeOffset.Now}");
			return Task.CompletedTask;
		}
	}
