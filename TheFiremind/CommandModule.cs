using Discord;
using Discord.Interactions;
using TheFiremind.Models;
using TheFiremind.Services;

namespace TheFiremind
{
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
            List<Embed> embeds = new();
            embeds.Add(new EmbedBuilder()
            {
                Title = "[square braces]",
                Description = "**[niv mizzet firemind]** will look at your message and response with a matching card's face for anything surrounded by square braces."
            }.Build());

            embeds.Add(new EmbedBuilder()
            {
                Title = "/oracle",
                Description = "Responds with the oracle text, mana cost, and face of the matching card"
            }.Build());

            embeds.Add(new EmbedBuilder()
            {
                Title = "/rulings",
                Description = "Responds with the matching card's face, rulings, their sources, and the dates they went into effect"
            }.Build());

            embeds.Add(new EmbedBuilder()
            {
                Title = "/card",
                Description = "Responds with the matching card's face"
            }.Build());

            await RespondAsync($"Commands", embeds.ToArray());
        }

        /// <summary>
        /// Slash command for looking up MTG card rulings
        /// </summary>
        [SlashCommand("rulings", RulingsCommandDescription)]
        public async Task RulingsAsync(string cardName)
        {
            IScryfallCard card;
            try
            {
                card = await _scryfall.GetCardAsync(cardName);
            }
            catch (Exception ex)
            {
                await RespondAsync(ex.Message);
                return;
            }

            ScryfallSingleObject<ScryfallRuling[]> scryfallSingleObject = await _scryfall.GetRulingsAsync(card.Id!);

            var rulings = scryfallSingleObject.Data!;

            if (rulings.Any())
            {
                var description = rulings.Select(r => $"*{r.Source}*\n> {r.Comment}\n`{r.Date:D}`\n\n")
                    .Aggregate((previous, current) => previous += current);

                var builder = new EmbedBuilder()
                {
                    Title = $"{card.Name}",
                    Description = description,
                    Url = card.Scryfall_Uri,
                    ImageUrl = card.Image_Uris!.Png
                };

                await RespondAsync("", new[] { builder.Build() });
            }
            else
            {
                await RespondAsync($"No rulings for [{card.Name}]");
            }
        }

        /// <summary>
        /// Slash command for looking up MTG card oracle text
        /// </summary>
        [SlashCommand("oracle", OracleCommandDescription)]
        public async Task OracleAsync(string cardName)
        {
            IScryfallCard card;
            try
            {
                card = await _scryfall.GetCardAsync(cardName);
            }
            catch (Exception ex)
            {
                await RespondAsync(ex.Message);
                return;
            }

            var builder = new EmbedBuilder()
            {
                Title = $"{card.Name}",
                Url = card.Scryfall_Uri,
                ImageUrl = card.Image_Uris!.Png,
                Fields = new()
                {
                    new()
                    {
                        Name = "Mana Cost",
                        Value = card.Mana_Cost,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Oracle Text",
                        Value = card.Oracle_Text,
                        IsInline = true
                    }
                }
            };

            await RespondAsync("", new[] { builder.Build() });
        }

        /// <summary>
        /// Slash command for looking up an MTG card face
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [SlashCommand("card", CardCommandDescription)]
        public async Task CardAsync(string name)
        {
            IScryfallCard card;
            try
            {
                card = await _scryfall.GetCardAsync(name);
            }
            catch (Exception ex)
            {
                await RespondAsync(ex.Message);
                return;
            }

            await RespondAsync(card.Image_Uris!.Png);
        }
    }
}
