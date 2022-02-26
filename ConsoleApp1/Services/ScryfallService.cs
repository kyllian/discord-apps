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
    internal class ScryfallClient
    {
        readonly RestClient restClient;
        public ScryfallClient(RestClient restClient, SettingsOptions settings)
        {
            var 
            this.restClient = restClient;
            this.restClient.
        }

        internal Card GetCard(string name)
        {

        }
    }
}
