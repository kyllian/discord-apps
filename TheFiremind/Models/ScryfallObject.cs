namespace TheFiremind.Models;

class ScryfallObject : IScryfallObject, IScryfallCard
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Code { get; init; }
    public string? Details { get; init; }
    public int? Status { get; init; }
    public string? Type { get; init; }
    public string Object { get; set; } = default!;

    internal bool IsCard => this.Object == "card";
    internal bool IsError => this.Object == "error";
}