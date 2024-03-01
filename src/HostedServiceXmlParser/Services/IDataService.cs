namespace HostedServiceXmlParser.Services;

public interface IDataService
	{
		Task<string> GetJsonDataAsync(Guid requestId, DateTimeOffset requestTime, string pathFile);
	}
