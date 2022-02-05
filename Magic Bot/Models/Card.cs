using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot.Models
{
    class Card
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("cmc")]
        public double cmc;
        [JsonProperty("image_uris")]
        public ImageUris uris;
        [JsonProperty("oracle_text")]
        public string oracleText;
        [JsonProperty("rulings_uri")]
        public string rulingsUri;
        [JsonProperty("mana_cost")]
        public string mana;
    }
}
