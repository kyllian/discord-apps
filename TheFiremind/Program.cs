using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Hosting;
using TheFiremind;
using TheFiremind.Services;

ReloadableLogger logger;

try
{
    logger = new LoggerConfiguration().CreateBootstrapLogger();
    Log.Logger = logger;

    Log.Debug("Created bootstrap logger");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to instantiate a bootstrap logger with the intended configuration");
    await Task.Delay(-1);
    return;
}

IHost host;

try
{
    host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((context, services) =>
            services.AddSingleton<DiscordSocketClient>()
                .AddSingleton(p => new InteractionService(p.GetRequiredService<DiscordSocketClient>(), new() { DefaultRunMode = RunMode.Async }))
                .AddOptions()
                .Configure<SettingsOptions>(context.Configuration.GetSection(nameof(SettingsOptions)))
                .AddTransient<ScryfallClient>()
                .AddTransient<CommandModule>()
                .AddSingleton<StartupService>())
        .Build();

    Log.Debug("Built application host");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to build the application host with the required configuration and services");
    await Task.Delay(-1);
    return;
}

using var scope = host.Services.CreateAsyncScope();
var provider = scope.ServiceProvider;

Log.Debug("Created an application scope for the startup routine");

var startup = provider.GetRequiredService<StartupService>();
logger.Reload(loggerConfiguration => loggerConfiguration.ReadFrom.Configuration(startup.Configuration));
Log.Debug("Reloaded the logger with appSettings configuration");

startup.RegisterSocketClientEventHandlers();
await startup.LoadModulesAsync();
await startup.ConnectToDiscordAsync();

host.Run();