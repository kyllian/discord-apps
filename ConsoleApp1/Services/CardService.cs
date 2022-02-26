using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }
    }
}
