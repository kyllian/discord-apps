namespace TheFiremind.Models;

interface IScryfallSingleObject<T> : IScryfallError
{
    public T? Data { get; init; }
}
