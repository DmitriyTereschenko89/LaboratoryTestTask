using System.Xml.Serialization;
using EventBus.Messages.Enums;
using EventBus.Messages.Models;
using Newtonsoft.Json;

namespace HostedServiceXmlParser.Services;

public class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly Dictionary<string, ModuleCategoryID> moduleCategoryMap = new()
    {
        ["SAMPLER"] = ModuleCategoryID.SAMPLER,
        ["QUATPUMP"] = ModuleCategoryID.QUATPUMP,
        ["COLCOMP"] = ModuleCategoryID.COLCOMP,
    };

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> GetJsonDataAsync(Guid requestId, DateTimeOffset requestTime, string pathFile)
    {
        var result = string.Empty;
        try
        {
            _logger.LogInformation($"{nameof(DataService)} is started processing file. CorrelitionId = {requestId}, DateTime = {requestTime}");
            var randomModuleStatus = new Random();
            XmlSerializer serializer = new(typeof(InstrumentStatus));
            var instrumentStatus = new InstrumentStatus();
            using (var stream = new FileStream(pathFile, FileMode.Open))
            {
                instrumentStatus = (InstrumentStatus)serializer.Deserialize(stream);
            }
            foreach (var item in instrumentStatus.DeviceStatuses)
            {
                item.RapidControlStatus.ModuleState = (ModuleState)randomModuleStatus.Next(3);
            }
            result = JsonConvert.SerializeObject(instrumentStatus.DeviceStatuses, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _logger.LogInformation($"{nameof(DataService)} is stoped processing file. CorrelitionId = {requestId}, DateTime = {DateTimeOffset.Now}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Request ID = {requestId}: Error = {ex.Message}");
        }

        return await Task.FromResult(result);
    }
}
