using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFiremind.Models;

class Card
{
    public string Name { get; set; } = default!;
    public double Cmc { get; set; }
    public ImageUris Uri { get; set; } = default!;
    public string OracleText { get; set; } = default!;
    public string RulingsUri { get; set; } = default!;
    public string Mana { get; set; } = default!;
}