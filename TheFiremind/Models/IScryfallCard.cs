namespace TheFiremind.Models;

interface IScryfallCard : IScryfallError, IScryfallObject
{
    internal string? Id { get; init; }
    internal string? Name { get; init; }
    internal string? Scryfall_Uri { get; set; }
    internal string? Layout { get; set; }
    internal ScryfallImageUris? Image_Uris { get; set; }
    internal ScryfallObject[]? Card_Faces { get; set; }
    internal string? Mana_Cost { get; set; }
    internal string? Oracle_Text { get; set; }

    internal int FaceCount => this.Card_Faces?.Length ?? 1;
}
