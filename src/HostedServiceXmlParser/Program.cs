using HostedServiceXmlParser;
using HostedServiceXmlParser.Job;

try
{
    CreateHostBuilder(args).Build().Run();
}
catch(TaskCanceledException)
{
    Console.WriteLine("Service is stoped.");
}


IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
			.ConfigureServices(services =>
				services.AddHostedService<XmlReader>());
