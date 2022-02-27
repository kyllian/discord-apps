namespace TheFiremind.Models;

class ScryfallSingleObject<T> : IScryfallSingleObject<T>
{
    public T? Data { get; init; }
    public string? Code { get; init; }
    public string? Details { get; init; }
    public int? Status { get; init; }
    public string? Type { get; init; }
}
