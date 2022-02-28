namespace TheFiremind.Models;

interface IScryfallCard : IScryfallError, IScryfallObject
{
    internal string? Id { get; init; }
    internal string? Name { get; init; }
    internal string? Scryfall_Uri { get; set; }
    internal ScryfallImageUris? Image_Uris { get; set; }
    internal string? Mana_Cost { get; set; }
    internal string? Oracle_Text { get; set; }
}
