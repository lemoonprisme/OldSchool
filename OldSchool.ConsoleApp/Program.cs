using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Background;
using OldSchool.Application.Services;
using OldSchool.ConsoleApp;
using OldSchool.Infrastructure;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json");
var config = configurationBuilder.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var applicationBuilder = Host.CreateApplicationBuilder();
applicationBuilder.Services.AddLogging(c => c.AddSeq());
applicationBuilder.Services.AddScoped<ISchoolService, SchoolService>();
applicationBuilder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
applicationBuilder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
applicationBuilder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

applicationBuilder.Services.AddHostedService<SchoolCreatorService>();
applicationBuilder.Services.AddHostedService<StudentCreator>();
applicationBuilder.Services.AddHostedService<ConsoleService>();


var host = applicationBuilder.Build();

var db = host.Services.GetRequiredService<ApplicationContext>().Database;
db.EnsureDeleted();
db.EnsureCreated();
await host.StartAsync();

