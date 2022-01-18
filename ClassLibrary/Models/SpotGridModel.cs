using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class SpotGridModel
    {
        public string? SpotLetter { get; set; }
        public int SpotNumber { get; set; }
        public GridSpotSatus Status { get; set; } = GridSpotSatus.Empty;
    }
}
