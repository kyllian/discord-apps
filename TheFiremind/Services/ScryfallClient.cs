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
    readonly RestClient _restClient;
    readonly SettingsOptions _settings;

    RestRequest CardRequest => new(_settings.ScryfallApiCardNamedFragment);
    RestRequest RulingsRequest => new(_settings.ScryfallApiRulingsFragment);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settingsOptions"></param>
    public ScryfallClient(IOptions<SettingsOptions> settingsOptions)
    {
        _settings = settingsOptions.Value;
        _restClient = new(new RestClientOptions(_settings.ScryfallApiBaseUri));
        _restClient.UseSystemTextJson(new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    internal async Task<ScryfallObject> GetCardAsync(string name, bool exact = true)
    {
        var request = CardRequest.AddQueryParameter(exact ? "exact" : "fuzzy", name);
        var response = await _restClient.ExecuteGetAsync<ScryfallObject>(request);
        return response.Data!;
    }

    internal async Task<ScryfallRuling[]> GetRulingsAsync(string id)
    {
        var o = await _restClient.GetAsync<ScryfallSingleObject<ScryfallRuling[]>>(RulingsRequest.AddUrlSegment("id", id));
        return o!.Data!;
    }
}
