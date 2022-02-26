using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TheFiremind.Modules;

/// <summary>
/// 
/// </summary>
public class ClientModule : InteractionModuleBase
{
    readonly DiscordSocketClient _client;
    readonly InteractionService _interactionService;
    readonly IHostEnvironment _environment;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="interactionService"></param>
    /// <param name="environment"></param>
    public ClientModule(DiscordSocketClient client, InteractionService interactionService, IHostEnvironment environment)
    {
        _client = client;
        _interactionService = interactionService;
        _environment = environment;

        RegisterHandlers();
    }

    void RegisterHandlers()
    {
        _client.Log += message =>
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(message.Exception, $"{nameof(LogSeverity.Critical)} {nameof(DiscordSocketClient)} Service Error - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Error:
                    Log.Fatal(message.Exception, $"{nameof(DiscordSocketClient)} Service {nameof(LogSeverity.Error)} - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Warning:
                    Log.Fatal(message.Exception, $"{nameof(DiscordSocketClient)} Service {nameof(LogSeverity.Warning)} - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Info:
                    Log.Information($"{nameof(DiscordSocketClient)} - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Verbose:
                    Log.Verbose($"{nameof(DiscordSocketClient)} - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Debug:
                    Log.Debug($"{nameof(DiscordSocketClient)} - {message.Source}; {message.Message}");
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        };

        _client.GuildAvailable += async guild =>
        {
            try
            {
                if (_environment.IsDevelopment())
                {
                    await _interactionService.RegisterCommandsToGuildAsync(guild.Id);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to register commands");
            }
        };

        _client.Connected += () =>
        {
            Log.Information("Connected to Discord");
            return Task.CompletedTask;
        };

        _client.Ready += () =>
        {
            Log.Information("Finished downloading guild data");
            return Task.CompletedTask;
        };
    }
}
