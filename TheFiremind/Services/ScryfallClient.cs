using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using TheFiremind.Models;

namespace TheFiremind.Services;

/// <summary>
/// 
/// </summary>
public class ScryfallClient
{
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
        var response = await RestClient.ExecuteGetAsync<T>(request);
        var errorObject = response.Data!;
        return errorObject.Object switch
        {
            "error" => throw new Exception(errorObject.Details),
            _ => errorObject,
        };
    }
}
