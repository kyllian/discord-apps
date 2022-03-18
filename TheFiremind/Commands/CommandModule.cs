using Discord;
using Discord.Interactions;
using TheFiremind.Handlers;
using TheFiremind.Models;
using TheFiremind.Services;

namespace TheFiremind.Commands;

/// <summary>
/// Command module facilitating <c>InteractionService</c> commands
/// </summary>
public class CommandModule : InteractionModuleBase<SocketInteractionContext>
{
    const string HelpCommandDescription = $"Get directions on how to use {nameof(TheFiremind)} bot";
    const string RulingsCommandDescription = "Search for an MTG card by name and look up its rulings";
    const string OracleCommandDescription = "Search for an MTG card by name and look up its oracle text";
    const string CardCommandDescription = "Search for an MTG card by name and look up its face";

    readonly ScryfallClient _scryfall;

    /// <summary>
    /// DI constructor
    /// </summary>
    /// <param name="scryfall"></param>
    public CommandModule(ScryfallClient scryfall)
    {
        _scryfall = scryfall;
    }

    /// <summary>
    /// Slash command for getting directions on how to use the bot
    /// </summary>
    [SlashCommand("help", HelpCommandDescription)]
    public async Task HelpAsync()
    {
        EmbedBuilder builder = new()
        {
            Title = $"{nameof(TheFiremind)} Help - Commands",
            Description = HelpCommandDescription,
            Fields = new()
            {
                new()
                {
                    Name = "[square braces]",
                    Value = "[niv mizzet firemind] will look at your message and respond with a matching card's face(s) for anything surrounded by square braces."
                },
                new()
                {
                    Name = "/oracle",
                    Value = "Responds with the oracle text, mana cost, and face(s) of the matching card"
                },
                new()
                {
                    Name = "/rulings",
                    Value = "Responds with the matching card's face(s), rulings, their sources, and the dates they went into effect"
                },
                new()
                {
                    Name = "/card",
                    Value = "Responds with the matching card's face(s)"
                },
            }
        };

        await RespondAsync(embed: builder.Build());
    }

    /// <summary>
    /// Slash command for looking up MTG card rulings
    /// </summary>
    [SlashCommand("rulings", RulingsCommandDescription)]
    public async Task RulingsAsync([Autocomplete(typeof(CardNameAutocompleteHandler))] string cardName)
    {
        try
        {
            var card = await _scryfall.GetCardAsync(cardName);
            var rulings = await _scryfall.GetRulingsAsync(card.Id!);
            var embeds = card.BuildEmbeds(rulings);
            await RespondAsync(embeds: embeds);
        }
        catch (Exception ex)
        {
            await RespondAsync(ex.Message);
        }
    }

    /// <summary>
    /// Slash command for looking up the MTG card oracle text
    /// </summary>
    [SlashCommand("oracle", OracleCommandDescription)]
    public async Task OracleAsync([Autocomplete(typeof(CardNameAutocompleteHandler))] string cardName)
    {
        IScryfallCard card;
        try
        {
            card = await _scryfall.GetCardAsync(cardName);
            var embeds = card.BuildEmbeds(Command.Oracle);
            await RespondAsync(embeds: embeds);
        }
        catch (Exception ex)
        {
            await RespondAsync(ex.Message);
            return;
        }
    }

    /// <summary>
    /// Slash command for looking up an MTG card
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [SlashCommand("card", CardCommandDescription)]
    public async Task CardAsync([Autocomplete(typeof(CardNameAutocompleteHandler))] string name)
    {
        try
        {
            var card = await _scryfall.GetCardAsync(name);
            var embeds = card.BuildEmbeds();
            await RespondAsync(embeds: embeds);
        }
        catch (Exception ex)
        {
            await RespondAsync(ex.Message);
        }
    }
}
