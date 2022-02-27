namespace TheFiremind.Models;

interface IScryfallError
{
    public string? Code { get; init; }
    public string? Details { get; init; }
    public int? Status { get; init; }
    public string? Type { get; init; }
}
