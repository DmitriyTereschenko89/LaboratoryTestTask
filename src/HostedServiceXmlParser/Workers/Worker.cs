using EventBus.Messages.Events;
using HostedServiceXmlParser.Messaging;
using HostedServiceXmlParser.Services;

namespace HostedServiceXmlParser.Workers;

public class Worker : IWorker
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IDataService _dataService;
    private readonly IMessageService _messageService;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IDataService parkService, IMessageService messageService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _dataService = parkService ?? throw new ArgumentNullException(nameof(parkService));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
        var requestTimeGapInMilliseconds = _configuration.GetValue<int>("APISettings:FrequencyOfDataChangeInSeconds");
        var requestsDelayInSeconds = _configuration.GetValue<int>("APISettings:FrequencyOfDataChangeInSeconds") * 1000;
        var directoryPath = _configuration.GetValue<string>("APISettings:DirectoryPath");
        var queryPathes = new Queue<string>();
        var uniqueFiles = new HashSet<string>();
        while (!cancellationToken.IsCancellationRequested)
        {
            var eventMessage = new XmlReaderEvent();

            try
            {
                var data = string.Empty;
                var periodEnd = DateTimeOffset.Now.AddSeconds(requestTimeGapInMilliseconds);
                var files = Directory.GetFiles(directoryPath).ToList();
                files.ForEach(file =>
                {
                    if (uniqueFiles.Add(file))
                    {
                        queryPathes.Enqueue(file);
                    }
                });
                while (DateTimeOffset.Now < periodEnd)
                {
                    var requestId = Guid.NewGuid();
                    var requestTime = DateTimeOffset.Now;
                    if (queryPathes.TryDequeue(out string path))
                    {
                        try
                        {
                            data = await _dataService.GetJsonDataAsync(requestId, requestTime, path);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"CorrelationId = {eventMessage.CorrelationId}. Request ID = {requestId}: Request at {requestTime} ended with exception. Exception message: {ex.Message}");
                        }
                    }


                    await Task.Delay(requestsDelayInSeconds);
                }
                if (string.IsNullOrEmpty(data))
                {
                    continue;
                }
                eventMessage.DeviceStatusesJson = data;

                await _messageService.PublishMessage(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CorrelationId = {eventMessage.CorrelationId}: Event at {eventMessage.CreationDate} ended with exception. Exception message: {ex.Message}");
            }
        }
    }
}
