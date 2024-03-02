using System.Configuration;
using DataProcessor.Entities;
using Microsoft.EntityFrameworkCore;
using SqLite;

namespace DataProcessor.Repositories;

public class DataContext : DbContext
{
	private readonly IConfiguration _configuration;

	public DataContext(IConfiguration configuration, DbContextOptions<DataContext> options) : base(options)
	{
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Database.EnsureCreated();
	}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var mySqLiteOptionsOptions = _configuration.GetSection("SqLite").Get<SqLiteOptions>() ??
			throw new SettingsPropertyNotFoundException($"{nameof(SqLiteOptions)} config is not configured");

		// Check if options are already configured
		if (!optionsBuilder.IsConfigured)
		{
            optionsBuilder.UseSqlite(mySqLiteOptionsOptions.GetConnectionString());
		}
	}

	public DbSet<DeviceStatusEntity> DeviceStatuses { get; set; }
}
