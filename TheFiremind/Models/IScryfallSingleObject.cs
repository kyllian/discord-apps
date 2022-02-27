namespace TheFiremind.Models;

interface IScryfallSingleObject<T> : IScryfallError, IScryfallObject
{
    internal T? Data { get; init; }
}
