using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot.Models
{
    class Ruling
    {
        [JsonProperty("source")]
        public string source;
        [JsonProperty("published_at")]
        public DateTime date;
        [JsonProperty("comment")]
        public string comment;
    }
}
