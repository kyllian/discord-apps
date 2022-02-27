namespace TheFiremind.Models;

class ScryfallObject : IScryfallCard
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Code { get; init; }
    public string? Details { get; init; }
    public int? Status { get; init; }
    public string? Type { get; init; }
}