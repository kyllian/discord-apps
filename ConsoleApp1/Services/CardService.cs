using RestSharp;
using TheFiremind.Models;
using TheFiremind.Options;

namespace TheFiremind.Services
{
    internal class CardService
    {
        readonly RestClient _restClient;
        readonly SettingsOptions _settings;

        public CardService(RestClient restClient, SettingsOptions settings)
        {
            this._restClient = restClient;
            this._settings = settings;
        }

        public Card Get(string name)
        {
            throw new NotImplementedException();
        }
    }
}
