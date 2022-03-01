using Discord;
using Discord.Interactions;
using TheFiremind.Services;

namespace TheFiremind;

/// <summary>
/// Handles card name autocompletion
/// </summary>
public class CardNameAutocompleteHandler : AutocompleteHandler
{
    private readonly ScryfallClient _scryfall;

    /// <summary>
    /// Constructs with required dependencies
    /// </summary>
    /// <param name="scryfall"></param>
    public CardNameAutocompleteHandler(ScryfallClient scryfall) => this._scryfall = scryfall;

    /// <summary>
    /// Implements abstract method <cref><c>AutocompleteHandler.GenerateSuggestionsAsync</c></cref>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="interaction"></param>
    /// <param name="parameter"></param>
    /// <param name="services"></param>
    /// <returns></returns>
    public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction interaction, IParameterInfo parameter, IServiceProvider services)
    {
        var option = interaction.Data.Current;
        var cardName = (string)option.Value;

        if (cardName.Length < 3)
        {
            return AutocompletionResult.FromSuccess();
        }

        var results = await _scryfall.GetAutocompleteAsync(cardName);

        if (results.Any())
        {
            return AutocompletionResult.FromSuccess(results.Select(r => new AutocompleteResult(r, r)));
        }
        else
        {
            return AutocompletionResult.FromSuccess();
        }
    }
}
