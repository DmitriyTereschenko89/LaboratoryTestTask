using AutoMapper;
using DataProcessor.Entities;
using DataProcessor.Repositories;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using Newtonsoft.Json;

namespace DataProcessor.Processing;

public class ProcessDataUpdate : IProcessDataUpdate
{
    private readonly ILogger<ProcessDataUpdate> _logger;
    private readonly IDataRepository _dataRepository;
    private readonly IMapper _mapper;

    public ProcessDataUpdate(ILogger<ProcessDataUpdate> logger, IDataRepository dataRepository, IMapper mapper)
    {
        _logger = logger;
        _dataRepository = dataRepository;
        _mapper = mapper;
    }

    public async Task<bool> ProcessAsync(XmlReaderEvent msg)
    {
        try
        {
            if (msg != null && !string.IsNullOrEmpty(msg.DeviceStatusesJson))
            {
                var deviceStatuses = JsonConvert.DeserializeObject<List<DeviceStatus>>(msg.DeviceStatusesJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                _logger.LogInformation("CorrelationId: {id}. Start saving data to SqLite database.", msg.CorrelationId);
                var items = _mapper.Map<List<ModelEntity>>(deviceStatuses);
                await _dataRepository.AddRangeAsync(items);
                return true;
            }

            _logger.LogWarning("CorrelationId: {id}. Null or empty array message received", msg?.CorrelationId);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CorrelationId: {id}. Error processing message {error}", msg?.CorrelationId, ex.Message);
            return false;
        }
    }
}
