using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFiremind.Models
{
    internal record Card(string Name, double Cmc, ImageUris Uri, string OracleText, string RulingsUri, string Mana);
    internal record ImageUris(string Png, string BorderCrop, string Normal);
}
