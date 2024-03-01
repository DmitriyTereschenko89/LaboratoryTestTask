using EventBus.Messages.Enums;
using EventBus.Messages.Models;
using Newtonsoft.Json;
using System.Xml;

namespace HostedServiceXmlParser.Services;

public class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly ICreateClass _createClass;
    private readonly Dictionary<string, ModuleCategoryID> moduleCategoryMap = new()
    {
        ["SAMPLER"] = ModuleCategoryID.SAMPLER,
        ["QUATPUMP"] = ModuleCategoryID.QUATPUMP,
        ["COLCOMP"] = ModuleCategoryID.COLCOMP,
    };

    public DataService(ILogger<DataService> logger, ICreateClass createClass)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createClass = createClass ?? throw new ArgumentNullException(nameof(createClass));
    }

    public async Task<string> GetJsonDataAsync(Guid requestId, DateTimeOffset requestTime, string pathFile)
    {
        var result = string.Empty;
        try
        {
            _logger.LogInformation($"{nameof(DataService)} is started processing file. CorrelitionId = {requestId}, DateTime = {requestTime}");
            List<DeviceStatus> deviceStatuses = new();
            XmlDocument xDoc = new();
            xDoc.Load(pathFile);
            XmlNodeList xmlNodeList = xDoc.DocumentElement.SelectNodes("/InstrumentStatus/DeviceStatus");
            foreach (XmlNode node in xmlNodeList)
            {
                var deviceStatus = new DeviceStatus();
                ModuleCategoryID moduleCategoryID = moduleCategoryMap[node.SelectSingleNode("ModuleCategoryID").InnerText];
                int indexWithinRole = Convert.ToInt32(node.SelectSingleNode("IndexWithinRole").InnerText);
                XmlNode rapidControlStatusNode = node.SelectSingleNode("RapidControlStatus");
                XmlNodeList nodeList = rapidControlStatusNode.ChildNodes;
                var rapidControlStatus = _createClass.GetClass(nodeList[0].Name, nodeList[0].ChildNodes);
                deviceStatus.ModuleCategoryID = moduleCategoryID;
                deviceStatus.IndexWithinRole = indexWithinRole;
                deviceStatus.RapidControlStatus = rapidControlStatus;
                deviceStatuses.Add(deviceStatus);
            }
            result = JsonConvert.SerializeObject(deviceStatuses, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _logger.LogInformation($"{nameof(DataService)} is stoped processing file. CorrelitionId = {requestId}, DateTime = {DateTimeOffset.Now}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Request ID = {requestId}: Error = {ex.Message}");
        }

        return await Task.FromResult(result);
    }
}
