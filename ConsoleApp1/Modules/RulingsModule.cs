using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Text;
using TheFiremind.Services;

namespace TheFiremind.Modules;

/// <summary>
/// Module facilitating a rulings command
/// </summary>
public class RulingsModule : InteractionModuleBase
{
    readonly ScryfallClient _scryfall;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scryfall"></param>
    internal RulingsModule(ScryfallClient scryfall)
    {
        _scryfall = scryfall;
    }

    /// <summary>
    /// 
    /// </summary>
    [SlashCommand("rulings", "Search for a card by name and look up the rulings for it")]
    public async Task RulingsAsync(string cardName)
    {
        var card = await _scryfall.GetCardAsync(cardName, false);
        var rulings = await _scryfall.GetRulingsAsync(card.Id);

        if (rulings.Any())
        {
            StringBuilder builder = new();
            foreach (var ruling in rulings)
            {
                builder.AppendLine($"From {ruling.Source}:");
                builder.AppendLine($"{ruling.Comment}");
                builder.AppendLine($"Dated {ruling.Date:D}\n");
            }

            await RespondAsync(builder.ToString());
        }
        else
        {
            await RespondAsync($"No rulings for [{card.Name}]");
        }
    }
}
