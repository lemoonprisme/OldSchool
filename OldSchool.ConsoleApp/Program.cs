using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OldSchool.Application.Services;
using OldSchool.Domain.Models;
using OldSchool.Infrastructure;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
configurationBuilder.AddJsonFile("appsettings.json");
var config = configurationBuilder.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var applicationBuilder = Host.CreateApplicationBuilder();

applicationBuilder.Services.AddLogging(c => c.AddSeq());
applicationBuilder.Services.AddScoped<ISchoolService, SchoolService>();
applicationBuilder.Services.AddScoped<ISchoolStore, SchoolDataBase>();
applicationBuilder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

var host = applicationBuilder.Build();

var db = host.Services.GetRequiredService<ApplicationContext>().Database;
db.EnsureDeleted();
db.EnsureCreated();

var stopProgram = new CancellationTokenSource();
var stopGenerating = CancellationTokenSource.CreateLinkedTokenSource(stopProgram.Token);
var tokenToStopGenerationg = stopGenerating.Token;
var tokenToStopProgram = stopProgram.Token;

var generatorTask = new Task(() =>
{
    while (!tokenToStopGenerationg.IsCancellationRequested)
    {
        host.Services.GetRequiredService<ISchoolService>().GenerateStudentInStore();
        Thread.Sleep(1000);
    }
}, tokenToStopGenerationg);
generatorTask.Start();
while (!tokenToStopProgram.IsCancellationRequested)
{
    var message = Console.ReadLine();
    if (!String.IsNullOrEmpty(message))
    {
        message = message.ToLower();
    }
    switch (message)
    {
        case "stop":
            stopProgram.Cancel();
            break;
        case "enough":
            stopGenerating.Cancel();
            break;
        case null:
            break;
        case "get students":
        {
            Console.Write("Write school Id: ");
            var schoolIdString = Console.ReadLine();
            int schoolId;
            if (int.TryParse(schoolIdString, out schoolId))
            {
                var studentsNames = host.Services
                    .GetRequiredService<ISchoolStore>().GetSchools() 
                    .Where(s => s.SchoolId == schoolId)
                    .SelectMany(s => s.Students.Select(f => f.Name));
                foreach (var name in studentsNames)
                {
                    Console.WriteLine(name);
                }
                
            }
            break;
        }
    }
}
generatorTask.Wait();