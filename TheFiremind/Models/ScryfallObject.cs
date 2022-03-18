namespace TheFiremind.Models;

class ScryfallObject : IScryfallObject, IScryfallCard
{
    public string Object { get; set; } = default!;
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Scryfall_Uri { get; set; }
    public string? Layout { get; set; }
    public ScryfallImageUris? Image_Uris { get; set; }
    public ScryfallObject[]? Card_Faces { get; set; }
    public string? Mana_Cost { get; set; }
    public string? Oracle_Text { get; set; }
    public string? Code { get; init; }
    public string? Details { get; init; }
    public int? Status { get; init; }
    public string? Type { get; init; }

    internal bool IsCard => this.Object == "card";
    internal bool IsError => this.Object == "error";
}