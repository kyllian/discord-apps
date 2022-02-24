using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicBot.Models
{
    class Ruling
    {
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}
