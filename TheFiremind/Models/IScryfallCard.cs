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

    internal bool IsTransform => this.Layout == "transform";
    internal bool IsModalDfc => this.Layout == "modal_dfc";
    internal bool IsDoubleFacedToken => this.Layout == "double_faced_token";
    internal int FaceCount => 1 + Convert.ToInt16(this.IsTransform || this.IsModalDfc || this.IsDoubleFacedToken);
}
