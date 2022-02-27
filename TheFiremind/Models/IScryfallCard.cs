namespace TheFiremind.Models;

interface IScryfallCard : IScryfallError
{
    public string? Id { get; init; }
    public string? Name { get; init; }
}
