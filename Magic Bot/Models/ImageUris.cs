using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot.Models
{
    class ImageUris
    {
        [JsonProperty("png")]
        public string png;
        [JsonProperty("border_crop")]
        public string border_crop;
        [JsonProperty("normal")]
        public string normal;
    }
}
