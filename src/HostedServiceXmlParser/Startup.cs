using HostedServiceXmlParser.Job;
using HostedServiceXmlParser.Messaging;
using HostedServiceXmlParser.Services;
using HostedServiceXmlParser.Workers;
using RabbitMQ;
using System.Configuration;

namespace HostedServiceXmlParser;

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
			var rabbitMqOptions = Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>() ??
				throw new SettingsPropertyNotFoundException($"{nameof(RabbitMqOptions)} config is not configured");
			var t = services.AddRabbitMQ(rabbitMqOptions);

			services.AddSingleton<IDataService, DataService>();
			services.AddSingleton<IWorker, Worker>();
			services.AddTransient<IMessageService, MessageService>();

			services.AddHostedService<XmlReader>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
		}
	}
