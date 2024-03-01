namespace HostedServiceXmlParser.Workers;

	public interface IWorker
	{
		Task DoWork(CancellationToken cancellationToken);
	}
