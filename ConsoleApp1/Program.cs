using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using Serilog;
using Serilog.Extensions.Hosting;
using TheFiremind;
using TheFiremind.Modules;
using TheFiremind.Services;

ReloadableLogger logger;

try
{
    logger = new LoggerConfiguration().CreateBootstrapLogger();
    Log.Logger = logger;
}
catch (Exception ex)
{
    Log.Error(ex, "Failed to instantiate a bootstrap logger with the intended configuration");
}

IHost host;

try
{
    host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
        .ConfigureServices((context, services) =>
            services.AddSingleton<DiscordSocketClient>()
                .AddSingleton(p => new InteractionService(p.GetRequiredService<DiscordSocketClient>(), new() { DefaultRunMode = RunMode.Async }))
                .AddTransient<ScryfallClient>()
                .AddOptions()
                .Configure<SettingsOptions>(context.Configuration.GetSection(nameof(SettingsOptions))))
        .Build();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to build the application host with the required configuration and services");
    await Task.Delay(-1);
    return;
}

using var scope = host.Services.CreateAsyncScope();
var provider = scope.ServiceProvider;

var configuration = provider.GetRequiredService<IConfiguration>();

string token;

try
{
    token = configuration.GetValue<string>("TheFiremindDiscordAuthToken");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Authorization error: Failed to get environment variable %TheFiremindDiscordAuthToken% and cannot connect to Discord");
    await host.WaitForShutdownAsync();
    return;
}

var client = provider.GetRequiredService<DiscordSocketClient>();

var interactionService = provider.GetRequiredService<InteractionService>();
await interactionService.AddModuleAsync<ClientModule>(provider);

try
{
    await client.LoginAsync(TokenType.Bot, token);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to connect to Discord");
    await host.WaitForShutdownAsync();
}

try
{
    await client.StartAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start the DiscordSocketClient");
    await host.WaitForShutdownAsync();
}

try
{
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal error while application was running");
    await host.WaitForShutdownAsync();
}