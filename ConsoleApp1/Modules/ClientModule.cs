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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="interactionService"></param>
    /// <param name="environment"></param>
    public ClientModule(DiscordSocketClient client, InteractionService interactionService, IHostEnvironment environment)
    {
        client.GuildAvailable += async guild =>
        {
            try
            {
                if (environment.IsDevelopment())
                {
                    await interactionService.RegisterCommandsToGuildAsync(guild.Id);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to register commands");
            }
        };

        client.Connected += () =>
        {
            Log.Information("Connected to Discord");
            return Task.CompletedTask;
        };

        client.Ready += () =>
        {
            Log.Information("Finished downloading guild data");
            return Task.CompletedTask;
        };
    }
}
