using EventBus.Messages.Events;

namespace HostedServiceXmlParser.Messaging;

	public interface IMessageService
	{
		Task PublishMessage(XmlReaderEvent msg);
	}
