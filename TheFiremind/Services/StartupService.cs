using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Text;
using System.Text.RegularExpressions;
using TheFiremind.Models;

namespace TheFiremind.Services;

class StartupService
{
    readonly DiscordSocketClient _client;
    readonly InteractionService _interactionService;
    readonly ScryfallClient _scryfall;
    readonly IHostEnvironment _environment;
    readonly IServiceProvider _services;

    string AuthToken => Configuration["TheFiremindDiscordAuthToken"];

    internal IConfiguration Configuration { get; }

    public StartupService(DiscordSocketClient client, InteractionService interactionService, ScryfallClient scryfallClient, IHostEnvironment environment, IServiceProvider services, IConfiguration configuration)
    {
        _client = client;
        _interactionService = interactionService;
        _scryfall = scryfallClient;
        _environment = environment;
        _services = services;

        Configuration = configuration;
    }

    internal void RegisterSocketClientEventHandlers()
    {
        _client.Log += message =>
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    Log.Fatal(message.Exception, $"{nameof(LogSeverity.Critical)} {nameof(DiscordSocketClient)} Service Error - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Error:
                    Log.Error(message.Exception, $"{nameof(DiscordSocketClient)} Service {nameof(LogSeverity.Error)} - {message.Source}; {message.Message}");
                    break;
                case LogSeverity.Warning:
                    Log.Warning(message.Exception, $"{nameof(DiscordSocketClient)} Service {nameof(LogSeverity.Warning)} - {message.Source}; {message.Message}");
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

        _client.InteractionCreated += async interaction =>
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(context, _services);
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

        _client.MessageReceived += MessageReceivedHandler;
    }

    internal async Task LoadModulesAsync() => await _interactionService.AddModuleAsync<CommandModule>(_services);

    internal async Task ConnectToDiscordAsync()
    {
        Log.Debug("Logging in to Discord...");
        await _client.LoginAsync(TokenType.Bot, AuthToken);
        await _client.StartAsync();
    }

    private async Task MessageReceivedHandler(SocketMessage message)
    {
        var content = message.Content;
        var author = message.Author;
        if (author.IsWebhook || author.IsBot || !(content.Contains('[') && content.Contains(']'))) return;

        var queries = Regex.Matches(content, "(?<=\\[)([^]]+)(?=\\])").Select(m => m.Value).Distinct().Where(c => !string.IsNullOrWhiteSpace(c));

        List<Embed> embeds = new();
        StringBuilder builder = new();
        foreach (var query in queries)
        {
            IScryfallCard card;
            try
            {
                card = await _scryfall.GetCardAsync(query);
                await message.Channel.SendMessageAsync(card.Image_Uris!.Png, messageReference: new(message.Id));
            }
            catch (Exception ex)
            {
                builder.Append($"{query}: {ex.Message}\n");
            }
        }

        await message.Channel.SendMessageAsync(builder.ToString(), messageReference: new(message.Id));
    }
}
