using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Interactions;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Extensions.Hosting;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using RestSharp;

ReloadableLogger logger;

try
{
    logger = new LoggerConfiguration()
        //.WriteTo.File("")
        .CreateBootstrapLogger();

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
        .ConfigureAppConfiguration(configuration => configuration.AddEnvironmentVariables())
        .ConfigureServices((context, services) =>
            services.AddSingleton<DiscordSocketClient>()
                .AddSingleton(p => new InteractionService(p.GetRequiredService<DiscordSocketClient>(), new() { DefaultRunMode = RunMode.Async }))
                .AddTransient<RestClient>())
        .UseSerilog((context, services, configuration) =>
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console())
        
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

var configuration = provider.GetRequiredService<EnvironmentVariablesConfigurationProvider>();

string token;
if (!configuration.TryGet("TheFiremindDiscordAuthToken", out token))
{
    Log.Fatal("Authorization error: Found no token in environment variable %TheFiremindDiscordAuthToken% and cannot connect to Discord");
    await host.WaitForShutdownAsync();
    return;
}

var client = provider.GetRequiredService<DiscordSocketClient>();

client.Log += message =>
{
    switch (message.Severity)
    {
        case global::Discord.LogSeverity.Critical:
            Log.Fatal(message.Exception, $"Critical DiscordSocketClient Service Error - Source: {message.Source}; {message.Message}");
            break;
        case global::Discord.LogSeverity.Error:
            Log.Fatal(message.Exception, $"DiscordSocketClient Service Error - Source: {message.Source}; {message.Message}");  
            break;
        case global::Discord.LogSeverity.Warning:
            Log.Fatal(message.Exception, $"DiscordSocketClient Service Warning - Source: {message.Source}; {message.Message}");
            break;
        case global::Discord.LogSeverity.Info:
            Log.Information($"DiscordSocketClient - Source: {message.Source}; {message.Message}");
            break;
        case global::Discord.LogSeverity.Verbose:
            Log.Verbose($"DiscordSocketClient - Source: {message.Source}; {message.Message}");
            break;
        case global::Discord.LogSeverity.Debug:
            Log.Debug($"DiscordSocketClient - Source: {message.Source}; {message.Message}");
            break;
        default:
            break;
    }

    return Task.CompletedTask;
};

try
{
    await client.LoginAsync(TokenType.Bot, token);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to connect to Discord");
    await host.WaitForShutdownAsync();
    return;
}

try
{
    await client.StartAsync();
}
catch (Exception ex)
{   
    Log.Fatal(ex, "Failed to start the DiscordSocketClient");
    await host.WaitForShutdownAsync();
    return;
}

try
{
    var interactionService = provider.GetRequiredService<InteractionService>();
    var environment = provider.GetRequiredService<IHostEnvironment>();

    if (environment.IsDevelopment())
    {
        await interactionService.RegisterCommandsToGuildAsync(234);
    }
    else
    {
        await interactionService.RegisterCommandsGloballyAsync();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to register commands");
    await host.WaitForShutdownAsync();
    return;
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
