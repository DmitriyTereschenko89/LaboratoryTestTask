using DataProcessor.Processing;
using DataProcessor.Repositories;
using Microsoft.EntityFrameworkCore;
using RabbitMQ;
using SqLite;
using System.Configuration;

namespace DataProcessor;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));
        var mySqLiteOptions = Configuration.GetSection("SqLite").Get<SqLiteOptions>() ??
            throw new SettingsPropertyNotFoundException($"{nameof(SqLiteOptions)} config is not configured");

        services.AddDbContext<DataContext>(options =>
            options.UseSqlite(mySqLiteOptions.GetConnectionString()), ServiceLifetime.Singleton);

        var rabbitMqOptions = Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>() ??
            throw new SettingsPropertyNotFoundException($"{nameof(RabbitMqOptions)} config is not configured");
        services.AddRabbitMQ(rabbitMqOptions);
        services.AddSingleton<IDataRepository, DataRepository>();
        services.AddSingleton<IProcessDataUpdate, ProcessDataUpdate>();

        services.AddHostedService<XmlReaderHost>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
