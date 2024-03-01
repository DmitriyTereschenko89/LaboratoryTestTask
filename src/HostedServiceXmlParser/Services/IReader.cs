namespace HostedServiceXmlParser.Services;

public interface IReader
{
    public Task<List<string>> ReadAsync(string path);
}
