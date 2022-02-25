using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFiremind.Models;

class ImageUris
{
    public string Png { get; set; } = default!;
    public string BorderCrop { get; set; } = default!;
    public string Normal { get; set; } = default!;
}
