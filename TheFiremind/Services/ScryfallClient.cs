using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;

namespace TheFiremind.Services;

class ScryfallClient
{
    readonly RestClient _restClient;
    readonly SettingsOptions _settings;

    RestRequest CardRequest => new(_settings.ScryfallApiCardNamedFragment);
    RestRequest RulingsRequest => new(_settings.ScryfallApiRulingsFragment);

    public ScryfallClient(IOptions<SettingsOptions> settingsOptions)
    {
        _settings = settingsOptions.Value;
        _restClient = new(new RestClientOptions(_settings.ScryfallApiBaseUri));
        _restClient.UseSystemTextJson(new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    internal async Task<Card> GetCardAsync(string name, bool exact = true)
    {
        return (await _restClient.GetAsync<Card>(CardRequest.AddQueryParameter(exact ? "exact" : "fuzzy", name)))!;
    }

    internal async Task<Ruling[]> GetRulingsAsync(string id)
    {
        var o = await _restClient.GetAsync<ScryfallSingleObject<Ruling[]>>(RulingsRequest.AddUrlSegment("id", id));
        return o!.Data;
    }
}
