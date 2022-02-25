using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFiremind.Models;

class Ruling
{
    public string Source { get; set; } = default!;
    public DateTime Date { get; set; } = default!;
    public string Comment { get; set; } = default!;
}
