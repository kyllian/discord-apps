namespace TheFiremind.Models;

interface IScryfallCard : IScryfallError, IScryfallObject
{
    internal string? Id { get; init; }
    internal string? Name { get; init; }
}
