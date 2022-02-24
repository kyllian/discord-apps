using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicBot.Models
{
    class Card
    {
        public string Name { get; set; }
        public double Cmc { get; set; }
        public ImageUris Uri { get; set; }
        public string OracleText { get; set; }
        public string RulingsUri { get; set; }
        public string Mana { get; set; }
    }
}
