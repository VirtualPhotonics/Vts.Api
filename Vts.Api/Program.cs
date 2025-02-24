using Vts.Api;

using var loggerFactory = LoggerFactory.Create(x => x
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

var logger = loggerFactory.CreateLogger<Startup>();

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration, logger);

startup.ConfigureServices(builder.Services);

var webApplication = builder.Build();

startup.Configure(webApplication, webApplication.Environment);

webApplication.Run();
