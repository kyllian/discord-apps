using Discord.Interactions;
using TheFiremind.Models;
using TheFiremind.Services;

namespace TheFiremind
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        readonly ScryfallClient _scryfall;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scryfall"></param>
        public CommandModule(ScryfallClient scryfall)
        {
            _scryfall = scryfall;
        }

        /// <summary>
        /// Responds with directions on how to use the bot
        /// </summary>
        [SlashCommand("h", $"Get directions on how to use {nameof(TheFiremind)} bot")]
        public async void HelpAsync()
        {
            await RespondAsync(@"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect");
        }

        /// <summary>
        /// 
        /// </summary>
        [SlashCommand("r", "Search for a card by name and look up its rulings")]
        public async Task RulingsAsync(string cardName)
        {
            ScryfallObject scryfallObject = await _scryfall.GetCardAsync(cardName, false);

            IScryfallCard card;
            switch (scryfallObject.Object)
            {
                case "error":
                    IScryfallError error = scryfallObject;
                    await RespondAsync(error.Details);
                    return;
                default:
                    card = scryfallObject;
                    break;
            }

            ScryfallSingleObject<ScryfallRuling[]> scryfallSingleObject = await _scryfall.GetRulingsAsync(card.Id!);

            var rulings = scryfallSingleObject.Data!;
            var message = rulings.Select(r => $"From {r.Source}:\n{r.Comment}\nDated {r.Date:D}\n\n")
                .Aggregate((previous, current) => previous += current);

            if (rulings.Any())
            {
                await RespondAsync(message);
            }
            else
            {
                await RespondAsync($"No rulings for [{card.Name}]");
            }
        }
    }
}
