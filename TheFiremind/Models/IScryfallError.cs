namespace TheFiremind.Models;

interface IScryfallError : IScryfallObject
{
    internal string? Code { get; init; }
    internal string? Details { get; init; }
    internal int? Status { get; init; }
    internal string? Type { get; init; }
}
