using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot.Models
{
    class RulingsList
    {
        [JsonProperty("data")]
        public Ruling[] rulings;
    }
}
