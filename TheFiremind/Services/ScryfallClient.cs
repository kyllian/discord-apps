using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using Serilog;
using System.Text.Json;
using TheFiremind.Models;

namespace TheFiremind.Services;

/// <summary>
/// 
/// </summary>
public class ScryfallClient
{
    private static SemaphoreSlim _pool = new(5, 5);
    private static int _numberQueued = 0;
    private static int _numberExecuting => 5 - _pool.CurrentCount;

    readonly SettingsOptions _settings;

    RestRequest CardRequest => new(_settings.ScryfallApiCardNamedFragment);
    RestRequest RulingsRequest => new(_settings.ScryfallApiRulingsFragment);
    RestRequest AutocompleteRequest => new(_settings.ScryfallApiAutocompleteFragment);
    RestClient RestClient
    {
        get
        {
            var client = new RestClient(new RestClientOptions(_settings.ScryfallApiBaseUri));
            return client.UseSystemTextJson(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settingsOptions"></param>
    public ScryfallClient(IOptions<SettingsOptions> settingsOptions)
    {
        _settings = settingsOptions.Value;
    }

    internal async Task<IScryfallCard> GetCardAsync(string name) => await GetScryfallCardObjectAsync(name, false);

    internal async Task<ScryfallRuling[]> GetRulingsAsync(string id) => (await GetScryfallRulingsObjectAsync(id));

    internal async Task<string[]> GetAutocompleteAsync(string query) => await this.GetScryfallAutocompleteObjectAsync(query);

    private async Task<ScryfallObject> GetScryfallCardObjectAsync(string name, bool exact)
    {
        var request = CardRequest.AddQueryParameter(exact ? "exact" : "fuzzy", name);
        return await this.GetAsync<ScryfallObject>(request);
    }

    private async Task<ScryfallRuling[]> GetScryfallRulingsObjectAsync(string id)
    {
        var request = RulingsRequest.AddUrlSegment("id", id);
        return (await this.GetAsync<ScryfallSingleObject<ScryfallRuling[]>>(request)).Data!;
    }

    private async Task<string[]> GetScryfallAutocompleteObjectAsync(string q)
    {
        var request = AutocompleteRequest.AddQueryParameter("q", q);
        return (await this.GetAsync<ScryfallSingleObject<string[]>>(request)).Data!;
    }

    private async Task<T> GetAsync<T>(RestRequest request) where T : IScryfallError
    {
        if (_numberExecuting > 4)
        {
            Log.Debug($"Scryfall requests queued: {Interlocked.Increment(ref _numberQueued)}");
        }

        await _pool.WaitAsync();
        if (_numberExecuting > 4)
        {
            Log.Debug($"Scryfall requests queued: {Interlocked.Decrement(ref _numberQueued)}");
        }

        Log.Debug($"Scryfall requests executing: {_numberExecuting}");

        try
        {
            var response = await RestClient.ExecuteGetAsync<T>(request);
            var errorObject = response.Data!;
            await Task.Delay(200 * _numberExecuting);
            return errorObject.Object switch
            {
                "error" => throw new Exception(errorObject.Details),
                _ => errorObject,
            };
        }
        finally
        {
            _pool.Release();
            Log.Debug($"Scryfall requests executing: {_numberExecuting}");
        }
    }
}
