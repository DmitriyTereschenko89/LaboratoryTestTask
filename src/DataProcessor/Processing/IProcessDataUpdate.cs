using EventBus.Messages.Events;

namespace DataProcessor.Processing;

	public interface IProcessDataUpdate
	{
		Task<bool> ProcessAsync(XmlReaderEvent msg);
	}
