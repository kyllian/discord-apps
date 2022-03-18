namespace TheFiremind;

/// <summary>
/// The Options Pattern model for application settings
/// </summary>
public class SettingsOptions
{
    /// <summary>
    /// The base URI for the Scryfall API
    /// </summary>
    public string ScryfallApiBaseUri { get; set; } = default!;

    /// <summary>
    /// The URI fragment for the Scryfall API Cards Named request
    /// </summary>
    public string ScryfallApiCardNamedFragment { get; set; } = default!;

    /// <summary>
    /// The URI fragment for the Scryfall API Cards Rulings request
    /// </summary>
    public string ScryfallApiRulingsFragment { get; set; } = default!;

    /// <summary>
    /// The URI fragment for the Scryfall API Cards Autocomplete request
    /// </summary>
    public string ScryfallApiAutocompleteFragment { get; set; } = default!;
}
