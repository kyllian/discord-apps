using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFiremind.Services;

namespace TheFiremind
{
    internal class CommandModule : InteractionModuleBase
    {
    readonly ScryfallClient _scryfall;

        public CommandModule(ScryfallClient scryfall)
        {
            _scryfall = scryfall;
        }

        /// <summary>
        /// Responds with directions on how to use the bot
        /// </summary>
        [SlashCommand("help", $"Get directions on how to use {nameof(TheFiremind)} bot")]
        public async void HelpAsync()
        {
            await RespondAsync(@"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect");
        }

        /// <summary>
        /// 
        /// </summary>
        [SlashCommand("rulings", "Search for a card by name and look up its rulings")]
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
}
