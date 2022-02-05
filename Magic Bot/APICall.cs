using Magic_Bot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot
{
    class APICall
    {
        public const string URL = "https://api.scryfall.com";
        private static HttpClient GetHttpClient(String url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async Task<Card> GetCardAsync(string name)
        {
            var urlParams = $"cards/named?fuzzy={name}";
            try
            {
                using (var client = GetHttpClient(URL))
                {
                    HttpResponseMessage response = await client.GetAsync(urlParams);
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Card>(json);
                        return result;
                    }
                    return default(Card);
                }
            }
            catch
            {
                return default(Card);
            }
        }
        public static async Task<RulingsList> GetRulings(string uri)
        {
            try
            {
                using (var client = GetHttpClient(uri))
                {
                    HttpResponseMessage response = await client.GetAsync("rulings");
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<RulingsList>(json);
                        return result;
                    }
                    return default(RulingsList);
                }
            }
            catch
            {
                return default(RulingsList);
            }
        }
    }
}
