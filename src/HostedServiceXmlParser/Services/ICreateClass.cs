using EventBus.Messages.Models;
using System.Xml;

namespace HostedServiceXmlParser.Services;

	public interface ICreateClass
	{
		RapidControlStatus GetClass(string className, XmlNodeList nodeList);
	}
